#Jetty

Jetty is a c# wrapper around the docker remote api. It can be used to manage your docker host remotely by creating/running/removing images and containers using c#.

##Example usage

```csharp

//connect to the docker remote
var client = new DockerClient("http://docker.local:2376");

//list all containers
var allContainers = await client.ListContainers();

//pull the images
var statusUpdate = await client.CreateImage("mono", "latest");

//create the container
var container = await client.CreateContainer(new CreateContainerOptions("mono", true, new [] { 80, 443 }));

//start the container we just created
var containerStartResponse = await client.StartContainer(container);
//test against your docker container
await client.KillContainer(container);
```
