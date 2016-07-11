using System;
using ReDock;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HelloWorld
{
    class MainClass
    {
        public static void Main(string[] args) => new MainClass().Run().Wait();

        public async Task Run()
        {
            Console.WriteLine("Finding hello-world image");
            string containerId = string.Empty;
            string imageId = string.Empty;

            //connect to the docker remote
            var client = new DockerClient("http://localhost:2376");
            
            //list all images
            var allImages = await client.ListImages(allImages: true);

            var image = allImages.FirstOrDefault(i => i.RepoTags.Contains("hello-world:latest"));

            if (image == null)
            {
                var data = await client.CreateImage("hello-world");

                Console.WriteLine($"{data.StatusErrors.Count} errors, {data.StatusUpdates.Count} status updates");
                if (data.State != CreateImageResultState.Error)
                {
                    imageId = data.ImageId;
                }
            }
            else
            {
                imageId = image.Id;
            }

            //list all containers
            var allContainers = await client.ListContainers(allContainers: true);
            
            var container = allContainers.FirstOrDefault(x => x.Image == "hello-world");
            if (container == null)
            {
                //host : client
                var portBindings = new Dictionary<int,int>();
                portBindings.Add(80, 80);
                portBindings.Add(443, 443);

                //create the container
                var containerResult = await client.CreateContainer(new CreateContainerOptions(imageId, true, 
                            portBindings: portBindings, bindings:new [] { "/etc/localtime:/etc/localtime:ro"}));
                containerId = containerResult.Id;
            }
            else
            {
                containerId = container.Id;  
            }
 
            Console.WriteLine("Starting hello-world image");

            //start the container we just created
            var containerStartResponse = await client.StartContainer(containerId);

            Console.WriteLine("Press any key to kill it");

            Console.ReadKey();

            var killResult = await client.KillContainer(containerId);
            Console.WriteLine(killResult.Result);

            Console.WriteLine("The container is dead as fried chicken!");

            Console.WriteLine("We don't want the container anymore, lets remove it");

            var removeContainerResult = await client.RemoveContainer(containerId, true, true);

            Console.ReadKey();

        }
    }
}
