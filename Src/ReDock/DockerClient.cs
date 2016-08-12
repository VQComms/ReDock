using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Linq;
using System.Net.Http;

namespace ReDock
{
    public class DockerClient
    {
        // using the docker remote api https://docs.docker.com/reference/api/docker_remote_api_v1.18/
        private readonly HttpClient client;
        
        public DockerClient(string Uri)
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(Uri);
        }

        public async Task<CreateImageResult> CreateImage(string imageName, string tag = null)
        {
            var queryStringDic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(imageName))
            {
                queryStringDic.Add("fromImage", imageName);
            }
            if (!string.IsNullOrEmpty(tag))
            {
                queryStringDic.Add("tag", tag);
            }
            var statusUpdates = new List<CreateImageStatusUpdate>();
            var statusErrors = new List<CreateImageStatusError>();
          
            var response = await this.client.PostAsJsonAsync("/images/create" + queryStringDic.ToQueryString(), new {});

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var updates = CreateImageStatusUpdate.FromString(line);
                    if (updates.Any(update => update.IsEmpty()))
                    {
                        var errors = CreateImageStatusError.FromString(line);
                        statusErrors.AddRange(errors.Where(err => !err.IsEmpty()));
                    }
                    statusUpdates.AddRange(updates.Where(up => !up.IsEmpty()));
                }
            }

            return await ParseCreateImageResult(imageName, tag, statusUpdates, statusErrors);
        }

        private async Task<CreateImageResult> ParseCreateImageResult(string imageName, string tag, List<CreateImageStatusUpdate> statusUpdates, List<CreateImageStatusError> errors)
        {
            var result = new CreateImageResult();
            if (statusUpdates.Any(x => x.status == "Download complete"))
            {
                result.State = CreateImageResultState.Created;
            }
            else if (statusUpdates.Any(x => x.status.Contains("Image is up to date")))
            {
                result.State = CreateImageResultState.AlreadyExists;
            }
            else if (errors.Any())
            {
                //is it safe to assume an error?
                result.State = CreateImageResultState.Error;
            }

            result.StatusUpdates = statusUpdates;
            result.StatusErrors = errors;
            if (result.State != CreateImageResultState.Error)
            {
                //go get the image id
                var image = await InspectImage(imageName, tag);
                result.ImageId = image.Id;
            }
            return result;
        }

        public async Task<ImageInspectResult> InspectImage(string imageName, string tag = null)
        {
            if (tag == null)
            {
                tag = "latest";
            }
            imageName = imageName + ":" + tag;

            var result = await client.GetAsync(string.Format("/images/{0}/json", imageName));
            return await result.Content.ReadAsJsonAsync<ImageInspectResult>();
        }

        public async Task<IEnumerable<Image>> ListImages(bool allImages = false)
        {
            var queryStringDic = new Dictionary<string, string>();

            queryStringDic.Add("all", allImages.ToString());

            var resp = await this.client.GetAsync("images/json" + queryStringDic.ToQueryString());

            var data = await resp.Content.ReadAsJsonAsync<List<Image>>();

            return data ?? new List<Image>();
        }

        public async Task<IEnumerable<Container>> ListContainers(bool allContainers = false)
        {
            var queryStringDic = new Dictionary<string, string>();

            queryStringDic.Add("all", allContainers.ToString());

            var resp = await this.client.GetAsync("/containers/json" + queryStringDic.ToQueryString());

            var data = await resp.Content.ReadAsJsonAsync<List<Container>>();

            return data ?? new List<Container>();
        }

        public async Task<CreateContainerResult> CreateContainer(CreateContainerOptions options, string containerName = "")
        {
            var requestUrl = String.IsNullOrEmpty(containerName) ? "/containers/create" : "/containers/create?name=" + containerName;
            var response = await this.client.PostAsJsonAsync(requestUrl, options);
            return await response.Content.ReadAsJsonAsync<CreateContainerResult>();
        }

        public async Task<StartContainerResult> StartContainer(string containerId)
        {
            var response = await this.client.PostAsJsonAsync(string.Format("/containers/{0}/start", containerId), new {});
            return new StartContainerResult(containerId, response.StatusCode);
        }

        public async Task<StartContainerResult> KillContainer(string containerId)
        {
            var response = await this.client.PostAsJsonAsync(string.Format("/containers/{0}/kill", containerId), new {});
            return new StartContainerResult(containerId, response.StatusCode);
        }


        /// <summary>
        /// Removes the container.
        /// </summary>
        /// <returns>The Result</returns>
        /// <param name="containerId">Container identifier.</param>
        /// <param name="removeVolumes">If set to <c>true</c> remove volumes.</param>
        /// <param name="force">If set to <c>true</c> force will kill and then remove the container</param>
        public async Task<RemoveContainerResult> RemoveContainer(string containerId, bool removeVolumes = false, bool force = false)
        {
            var queryStringDic = new Dictionary<string, string>();

            var url = string.Format("/containers/{0}", containerId);
          
            queryStringDic.Add("v", removeVolumes.ToString());
            queryStringDic.Add("force", force.ToString());

            url += queryStringDic.ToQueryString();

            var response = await this.client.DeleteAsync(url);

            return new RemoveContainerResult()
            {
                State = (RemoveContainerResultState)((int)response.StatusCode)
            };
        }
    }
}
