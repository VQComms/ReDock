using System;
using System.Collections.Generic;

namespace ReDock
{
    public class CreateContainerOptions
    {
        public readonly string ContainerName;
        public readonly string Image;
        public readonly bool Tty;
        public readonly Dictionary<string, object> ExposedPorts;

        public readonly HostConfig HostConfig;
        public CreateContainerOptions(string imageId, bool tty, IEnumerable<int> ports = null, string containerName = "", HostConfig hostConfig = null)
        {
            this.ContainerName = containerName;
            this.Image = imageId;
            this.Tty = tty;
            this.HostConfig = hostConfig;
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
