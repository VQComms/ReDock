[![Build status](https://ci.appveyor.com/api/projects/status/srwhwk2af60ikakc/branch/master?retina=true&passingText=master)](https://ci.appveyor.com/project/VQCommunications/redock/branch/master)
[![NuGet](http://img.shields.io/nuget/v/ReDock.svg?style=flat)](https://www.nuget.org/packages/reDock/)

#ReDock
ReDock is a C# wrapper around the docker remote API. It can be used to manage your docker host remotely by creating/running/removing images and containers using C#.

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
var containerStartResponse = await client.StartContainer(container.Id);

//kill your docker container
await client.KillContainer(container.Id);
```
