using System.Collections.Generic;

namespace ReDock
{
    public class PortMapping : List<Dictionary<string,string>>
	{
        public PortMapping(int hostPort)
        {
            var mapping = new Dictionary<string,string>();
            mapping.Add("HostPort", hostPort.ToString());//damn the docker objects in the api are annoying to construct :P there must be a better way of doing this
            mapping.Add("HostIP", "0.0.0.0");

            this.Add(mapping);
        }
	}
}