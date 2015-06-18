using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ReDock
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
            return string.Format("[CreateImageStatusUpdate: Id={0}, Status={1}]", Id, Status);
        }
    }
}
