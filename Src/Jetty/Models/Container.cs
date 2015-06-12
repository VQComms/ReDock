using System.Collections.Generic;

namespace Jetty
{
    public class Container
    {
        public string Id { get; set; }

        public string Image { get; set; }

        public string Command { get; set; }

        public int Created { get; set; }

        public string Status { get; set; }

        public List<ContainerPort> Ports { get; set; }

        public int SizeRw { get; set; }

        public int SizeRootFs { get; set; }
    }
}