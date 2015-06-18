using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using System.IO;
using System;
using Newtonsoft.Json;

namespace ReDock
{
	public class CreateImageResult
	{
        public List<CreateImageStatusUpdate> StatusUpdates { get; set; }

        public string ImageId { get; set; }

        public CreateImageResultState State { get; set; }
	}

}
