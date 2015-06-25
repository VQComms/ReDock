using System;
using ReDock;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HelloWorld
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Finding hello-world image");
            string containerId = string.Empty;
            string imageId = string.Empty;

            //connect to the docker remote
            var client = new DockerClient("http://docker.local:2376");
          
            //list all images
            var allImages = client.ListImages(allImages: true).Result;

            var image = allImages.FirstOrDefault(i => i.RepoTags.Contains("hello-world:latest"));

            if (image == null)
            {
                var data = client.CreateImage("hello-world").Result;
                if (data.State != CreateImageResultState.Error)
                {
                    imageId = data.ImageId;
                }
            }

            //list all containers
            var allContainers = client.ListContainers(allContainers: true).Result;

            var container = allContainers.FirstOrDefault(x => x.Image == "hello-world");

            if (container == null)
            {
                //create the container
                var containerResult = client.CreateContainer(new CreateContainerOptions(imageId, true)).Result;
                containerId = containerResult.Id;
            }
            else
            {
                containerId = container.Id;  
            }
 
            Console.WriteLine("Starting hello-world image");

            //host : client
            var portMappings = new Dictionary<int,int>();
            portMappings.Add(80, 80);
            portMappings.Add(443, 443);

            //start the container we just created
            var containerStartResponse = client.StartContainer(containerId, new ContainerHostConfig(portMappings)).Result;

            Console.WriteLine("Press any key to kill it");

            Console.ReadKey();

            client.KillContainer(containerId);

            Console.WriteLine("The container is dead as fried chicken!");

            Console.WriteLine("We don't want the container anymore, lets remove it");

            var removeContainerResult = client.RemoveContainer(containerId, true, true).Result;

            Console.ReadKey();

        }
    }
}
