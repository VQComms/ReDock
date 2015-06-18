using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using System.IO;
using System;
using Newtonsoft.Json;

namespace ReDock
{
    public enum CreateImageResultState
    {
        Error,
        AlreadyExists,
        Created
    }

}
