using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ReDock
{

    public class CreateImageStatusError
    {
        public ErrorDetail ErrorDetail { get; set; }

        public string Error { get; set; }

        public bool IsEmpty()
        {
            return ErrorDetail == null && string.IsNullOrEmpty(Error);
        }

        public static IEnumerable<CreateImageStatusError> FromString(string s)
        {
            var list = new List<CreateImageStatusError>();
            var splitArr = s.Split(new [] { "}{" }, StringSplitOptions.None);

            if (splitArr.Count() == 1)
            {

                var result = JsonConvert.DeserializeObject<CreateImageStatusError>(s);
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

                var data = JsonConvert.DeserializeObject<List<CreateImageStatusError>>(s);
                return data;
            }
        }
    }
    
}
