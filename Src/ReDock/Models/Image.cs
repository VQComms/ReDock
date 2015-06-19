using System;
using System.Collections.Generic;

namespace ReDock
{
    public class Image
    {
        public DateTime Created { get; set; }

        public string Id { get; set; }

        public string ParentId { get; set; }

        public List<string> RepoDigests { get; set; }

        public List<string> RepoTags { get; set; }

        public int Size { get; set; }

        public int VirtualSize { get; set; }
    }
}
