using System.Collections.Generic;

namespace ReDock
{
    public class CreateContainerOptions
    {        
        public readonly string Image;
        public readonly bool Tty;
        public readonly Dictionary<string, object> ExposedPorts;
        public readonly IList<string> Env;

        public readonly HostConfig HostConfig;
        public CreateContainerOptions(string imageId, bool tty, Dictionary<int,int> portBindings = null,bool publishAllPorts = false, IEnumerable<string> bindings = null, Dictionary<string,string> env = null)
        {            
            this.Image = imageId;
            this.Tty = tty;
            this.HostConfig = new HostConfig(bindings, portBindings, publishAllPorts);
            ExposedPorts = new Dictionary<string,object>();
            if (portBindings != null)
            {
                foreach (var item in portBindings)
                {
                    ExposedPorts.Add(string.Format("{0}/tcp", item.Key), new object());
                }
            }
            if (env != null)
            {
                Env = new List<string>();
                foreach (var kvp in env)
                {
                    Env.Add(string.Format("{0}={1}", kvp.Key, kvp.Value));
                }
            }
        }
    }
}
