using System.Collections.Generic;

namespace ReDock
{
    public class Container
    {
        public string Id { get; set; }
        
        public string[] Names { get; set; }

        public string Image { get; set; }
        
        public string ImageId { get; set; }

        public string Command { get; set; }

        public int Created { get; set; }

        public string Status { get; set; }
        
        public string State { get; set; }

        public List<ContainerPort> Ports { get; set; }

        public int SizeRw { get; set; }

        public int SizeRootFs { get; set; }
    }
}