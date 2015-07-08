using System;
using System.Collections.Generic;
using System.Linq;

namespace ReDock
{

    public class HostConfig
    {
        public List<string> Binds { get; set; }

        public HostConfig(IEnumerable<string> binds)
        {
            this.Binds = binds.ToList();
        }
    }
    
}
