using System.Net;

namespace ReDock
{
    public class StartContainerResult : ContainerActionResult
    {
        public StartContainerResult(string containerId, HttpStatusCode code)
            : base(containerId)
        {
            switch (code)
            {
                case HttpStatusCode.NoContent:
                    {
                        Result = ContainerResultState.NoError;
                        break;
                    }
                case HttpStatusCode.NotModified:
                    {
                        Result = ContainerResultState.ContainerAlreadyStarted;
                        break;
                    }
                case HttpStatusCode.NotFound:
                    {
                        Result = ContainerResultState.NoSuchContainer;
                        break;
                    }
                default:
                    {
                        Result = ContainerResultState.ServerError;
                        break;
                    }
            }
        }
    }
}
