using System.Net;

namespace Jetty
{

    public class KillContainerResult : ContainerActionResult
    {
        public KillContainerResult(string containerId, HttpStatusCode code) : base(containerId)
        {
            switch (code)
            {
                case HttpStatusCode.Created:
                    {
                        Result = ContainerResultState.NoError;
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
