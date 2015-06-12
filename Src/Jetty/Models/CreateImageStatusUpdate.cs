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
            var list = new List<CreateImageStatusUpdate>();
            var splitArr = s.Split(new [] { "}{" }, StringSplitOptions.None);

            if (splitArr.Count() == 1)
            {
                list.Add(JsonConvert.DeserializeObject<CreateImageStatusUpdate>(s));
                return list;
            }
            else
            {
                s = "[" + s + "]";

                s = s.Replace("}{", "},{");

                var data = JsonConvert.DeserializeObject<List<CreateImageStatusUpdate>>(s);
                return data;
            }
        }
    }
}
