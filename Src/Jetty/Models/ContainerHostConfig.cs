using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jetty
{
    public class ContainerHostConfig
    {
        public Dictionary<string,PortMapping> PortBindings { get; set; }
        public bool PublishAllPorts { get; set; }

        public ContainerHostConfig(Dictionary<int, int> portBindings, bool publishAllPorts = false)
        {
            this.PublishAllPorts = publishAllPorts;
            this.PortBindings = new Dictionary<string,PortMapping>();
            foreach (var binding in portBindings)
            {
                this.PortBindings.Add(string.Format("{0}/tcp",binding.Key), new PortMapping(binding.Value));
            }
        }
    }
}

