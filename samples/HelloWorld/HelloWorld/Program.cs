using System;
using Jetty;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HelloWorld
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Getting hello-world image");
            string containerId = string.Empty;

            //connect to the docker remote
            var client = new DockerClient("http://docker.local:2376");
          
            //list all containers
            var allContainers = client.ListContainers(allContainers: true).Result;

            var container = allContainers.FirstOrDefault(x => x.Image == "hello-world");

            if (container == null)
            {
                //Find image locally or pull it - this returns a list of statuses from creating it
                var data = client.CreateImage("hello-world").Result;
   
                Console.WriteLine("Downloading hello-world image");

                if (data.Any(x => x.Status == "Download complete") || data.Any(x => x.Status.Contains("Image is up to date")))
                {
                    //create the container
                    var containerResult = client.CreateContainer(new CreateContainerOptions("hello-world", true)).Result;
                    containerId = containerResult.Id;
                }
            }
            else
            {
                containerId = container.Id;  
            }
 
            Console.WriteLine("Starting hello-world image");

            //host : client
            var PortBindings = new Dictionary<int,int>();
            PortBindings.Add(80, 80);
            PortBindings.Add(443, 443);

            //start the container we just created
            var containerStartResponse = client.StartContainer(containerId, new ContainerHostConfig(PortBindings)).Result;

            Console.WriteLine("Press any key to kill it");

            Console.ReadKey();

            client.KillContainer(containerId);

            Console.WriteLine("The container is dead as fried chicken!");
            Console.ReadKey();

        }
    }
}
