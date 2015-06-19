using System.Net;

namespace ReDock
{
    public class StartContainerResult : ContainerActionResult
    {
        public StartContainerResult(string containerId, HttpStatusCode code)
            : base(containerId)
        {
            Result = (ContainerResultState)((int)code);
        }
    }
}
