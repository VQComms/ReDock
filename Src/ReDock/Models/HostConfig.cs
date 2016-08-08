using System;
using System.Collections.Generic;
using System.Linq;

namespace ReDock
{
    public class PortBindingDictionary : Dictionary<string,PortMapping>{}
    public class HostConfig
    {
        public PortBindingDictionary PortBindings { get; set; }
        
        public bool PublishAllPorts { get; set; }

        public readonly List<string> Binds;

        public HostConfig(IEnumerable<string> binds, Dictionary<int, int> portBindings, bool publishAllPorts = false)
        {
            if (binds != null)
            {
                this.Binds = binds.ToList();
            }
            this.PublishAllPorts = publishAllPorts;
            this.PortBindings = new PortBindingDictionary();
            foreach (var binding in portBindings)
            {
                this.PortBindings.Add(string.Format("{0}/tcp",binding.Key), new PortMapping(binding.Value));
            }
        }
    }
    
}
