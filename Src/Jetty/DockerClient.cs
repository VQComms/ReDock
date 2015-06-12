using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using System.IO;

namespace Jetty
{
    public class DockerClient
    {
        private IRestClient Client { get; set; }

        private string Uri { get; set; }

        // using the docker remote api https://docs.docker.com/reference/api/docker_remote_api_v1.18/

        public DockerClient(string Uri)
        {
            this.Uri = Uri;
            this.Client = new RestClient(Uri);
        }

        public async Task<IList<CreateImageStatusUpdate>> CreateImage(string imageName, string tag = null, string registry = null)
        {
            var request = new RestRequest("/images/create");

            if (!string.IsNullOrEmpty(imageName))
            {
                request.AddQueryParameter("fromImage", imageName);
            }
            if (!string.IsNullOrEmpty(registry))
            {
                request.AddQueryParameter("registry", registry);
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
                    string line = reader.ReadLine();
                    statusUpdates.AddRange(CreateImageStatusUpdate.FromString(line));
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        statusUpdates.AddRange(CreateImageStatusUpdate.FromString(line));
                    }
                }
            };

            await Client.ExecutePostTaskAsync<List<CreateImageStatusUpdate>>(request);
            return statusUpdates;
        }


        public async Task<IEnumerable<Container>> ListContainers()
        {
            var request = new RestRequest("/containers/json", Method.GET);
            var response = await Client.ExecuteGetTaskAsync<List<Container>>(request);
            return response.Data;
        }

        public async Task<CreateContainerResult> CreateContainer(CreateContainerOptions options)
        {
            var request = new RestRequest("/containers/create", Method.POST);
            request.AddJsonBody(options);
            var response = await Client.ExecutePostTaskAsync<CreateContainerResult>(request);

            return response.Data;

        }

        public async Task<StartContainerResult> StartContainer(CreateContainerResult container)
        {
            var request = new RestRequest(string.Format("/containers/{0}/start", container.Id), Method.POST);
            request.AddJsonBody(new object() { });
            var response = await Client.ExecutePostTaskAsync(request);

            return new StartContainerResult(container.Id, response.StatusCode);
        }

        public async Task<StartContainerResult> KillContainer(CreateContainerResult container)
        {
            var request = new RestRequest(string.Format("/containers/{0}/kill", container.Id), Method.POST);


            var response = await Client.ExecutePostTaskAsync(request);

            return new StartContainerResult(container.Id, response.StatusCode);
        }

    }
}
