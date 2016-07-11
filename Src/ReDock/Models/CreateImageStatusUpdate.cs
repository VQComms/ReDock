using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ReDock
{
    public class CreateImageStatusUpdate
    {
        public string id { get; set; }

        public string status { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(status) && string.IsNullOrEmpty(status);
        }

        public static IEnumerable<CreateImageStatusUpdate> FromString(string s)
        {
            var list = new List<CreateImageStatusUpdate>();
            var splitArr = s.Split(new [] { "}{" }, StringSplitOptions.None);

            if (splitArr.Count() == 1)
            {
                
                var result = JsonConvert.DeserializeObject<CreateImageStatusUpdate>(s);
                if (result != null)
                {
                    list.Add(result);
                }
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

        public override string ToString()
        {
            return string.Format("[CreateImageStatusUpdate: id={0}, status={1}]", id, status);
        }
    }
}
