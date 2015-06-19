using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Linq;

namespace ReDock
{
    public class DockerClient
    {
        // using the docker remote api https://docs.docker.com/reference/api/docker_remote_api_v1.18/
        private readonly IRestClient client;

        public DockerClient(string Uri)
        {
            this.client = new RestClient(Uri);
        }

        public async Task<CreateImageResult> CreateImage(string imageName, string tag = null)
        {
            var request = new RestRequest("/images/create");

            if (!string.IsNullOrEmpty(imageName))
            {
                request.AddQueryParameter("fromImage", imageName);
            }
            if (!string.IsNullOrEmpty(tag))
            {
                request.AddQueryParameter("tag", tag);
            }
            var statusUpdates = new List<CreateImageStatusUpdate>();
            request.ResponseWriter = (stream) =>
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        statusUpdates.AddRange(CreateImageStatusUpdate.FromString(line));
                    }
                }
            };
                    
            await this.client.ExecutePostTaskAsync<List<CreateImageStatusUpdate>>(request);

            return await ParseCreateImageResult(imageName, statusUpdates);
        }

        public async Task<RemoveContainerResult> RemoveContainer(string containerId, bool removeVolumes = false, bool force = false)
        {
            var request = new RestRequest(string.Format("/containers/{0}", containerId), Method.DELETE);
            request.AddQueryParameter("v", removeVolumes.ToString());
            request.AddQueryParameter("force", force.ToString());

            var result = await this.client.ExecuteTaskAsync(request);

            return new RemoveContainerResult()
            {
                State = (RemoveContainerResultState)((int)result.StatusCode)
            };

        }

        private async Task<CreateImageResult> ParseCreateImageResult(string imageName, List<CreateImageStatusUpdate> statusUpdates)
        {
            var result = new CreateImageResult();
            if (statusUpdates.Any(x => x.Status == "Download complete"))
            {
                result.State = CreateImageResultState.Created;
            }
            else if (statusUpdates.Any(x => x.Status.Contains("Image is up to date")))
            {
                result.State = CreateImageResultState.AlreadyExists;
            }
            else
            {
                //is it safe to assume an error?
                result.State = CreateImageResultState.Error;
            }

            result.StatusUpdates = statusUpdates;
            if (result.State != CreateImageResultState.Error)
            {
                //go get the image Id
                var image = await InspectImage(imageName);
                result.ImageId = image.Id;
            }
            return result;
        }

        public async Task<ImageInspectResult> InspectImage(string imageName)
        {
            var request = new RestRequest(string.Format("/images/{0}/json", imageName));

            var result = await client.ExecuteGetTaskAsync<ImageInspectResult>(request);

            return result.Data;
        }

        public async Task<IEnumerable<Container>> ListContainers(bool allContainers = false)
        {
            var request = new RestRequest("/containers/json", Method.GET);

            if (allContainers)
            {
                request.AddQueryParameter("all", "1");
            }
                
            var response = await this.client.ExecuteGetTaskAsync<List<Container>>(request);

            return response.Data ?? new List<Container>();
        }

        public async Task<CreateContainerResult> CreateContainer(CreateContainerOptions options)
        {
            var request = new RestRequest("/containers/create", Method.POST);
            request.AddJsonBody(options);
            var response = await this.client.ExecutePostTaskAsync<CreateContainerResult>(request);

            return response.Data;
        }

        public async Task<StartContainerResult> StartContainer(string containerId, ContainerHostConfig config = null)
        {
            var request = new RestRequest(string.Format("/containers/{0}/start", containerId), Method.POST);
            var body = JsonConvert.SerializeObject(config);
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var response = await this.client.ExecutePostTaskAsync(request);

            return new StartContainerResult(containerId, response.StatusCode);
        }

        public async Task<StartContainerResult> KillContainer(string containerId)
        {
            var request = new RestRequest(string.Format("/containers/{0}/kill", containerId), Method.POST);
            var response = await this.client.ExecutePostTaskAsync(request);

            return new StartContainerResult(containerId, response.StatusCode);
        }
    }
}
