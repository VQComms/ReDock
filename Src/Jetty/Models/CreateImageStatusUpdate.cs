using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Jetty
{
    public class CreateImageStatusUpdate
    {
        public string Id { get; set; }

        public string Status { get; set; }

        public static IEnumerable<CreateImageStatusUpdate> FromString(string s)
        {
            var splitArr = s.Split(new [] { "}{" }, StringSplitOptions.None);
            if (splitArr.Count() == 1)
            {
                JsonConvert.DeserializeObject<CreateImageStatusUpdate>(s);
            }
            else
            {
                foreach (var item in splitArr.Take(splitArr.Count() -1))
                {
                    var statusItem = JsonConvert.DeserializeObject<CreateImageStatusUpdate>(item + "}");
                    yield return statusItem;
                }
                yield return JsonConvert.DeserializeObject<CreateImageStatusUpdate>(splitArr.Last() + "}");
            }
        }
    }
}
