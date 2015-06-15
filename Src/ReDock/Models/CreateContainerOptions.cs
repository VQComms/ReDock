using System;
using System.Collections.Generic;

namespace ReDock
{
    public class CreateContainerOptions
    {
        public string ContainerName { get; set; }
        public string Image { get; set; }
        public bool Tty { get; set; }
        public Dictionary<string, object> ExposedPorts { get; set; }
        public CreateContainerOptions(string imageId, bool tty, IEnumerable<int> ports = null, string containerName = "")
        {
            this.ContainerName = containerName;
            this.Image = imageId;
            this.Tty = tty;
            ExposedPorts = new Dictionary<string,object>();
            if (ports != null)
            {
                foreach (var item in ports)
                {
                    ExposedPorts.Add(string.Format("{0}/tcp", item), new object());
                }
            }
        }
    }
}
