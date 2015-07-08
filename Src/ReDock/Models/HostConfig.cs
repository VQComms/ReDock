using System;
using System.Collections.Generic;
using System.Linq;

namespace ReDock
{

    public class HostConfig
    {
        public readonly List<string> Binds;

        public HostConfig(IEnumerable<string> binds)
        {
            this.Binds = binds.ToList();
        }
    }
    
}
