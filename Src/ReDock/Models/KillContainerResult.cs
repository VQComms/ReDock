using System.Net;

namespace ReDock
{
    public class KillContainerResult : ContainerActionResult
    {
        public KillContainerResult(string containerId, HttpStatusCode code) : base(containerId)
        {
            Result = (ContainerResultState)((int)code);
        }
    }
}
