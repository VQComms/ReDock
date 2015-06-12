#Jetty

Jetty is a c# wrapper around the docker remote api. It can be used to manage your docker host remotely by creating/running/removing images and containers using c#.

##Example usage

```csharp

//connect to the docker remote
var client = new DockerClient("http://docker.local:2376");

//list all containers
var allContainers = client.ListContainers().Result;

//pull the images
var statusUpdate = client.CreateImage("mono", "latest").Result;

//create the container
var container = client.CreateContainer(new CreateContainerOptions("mono", true, new [] { 80, 443 })).Result;

//start the container we just created
var containerStartResponse = await client.StartContainer(container);
//test against your docker container
await client.KillContainer(container);
```
