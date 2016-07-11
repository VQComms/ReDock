using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ReDock
{
    public static class DictionaryExtensions
    {
        public static string ToQueryString(this Dictionary<string, string> dic)
        {
            var qs = dic.Select(x => string.Format("{0}={1}", WebUtility.UrlEncode(x.Key), WebUtility.UrlEncode(x.Value)));

            return "?" + string.Join("&", qs);
        }
    }
}