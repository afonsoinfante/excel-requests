using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requests
{
    public class Accessor
    {
        private static char delimiter = '/';

        private static JToken get(object o, string key)
        {
            if (o is JArray)
                return (o as JArray)[Int32.Parse(key)];

            if (o is JObject)
                return (o as JObject)[key];

            return null;

        }

        public static JToken Get(JToken o, string path)
        {
            var parts = path.Split(delimiter);
            var value = o;
            foreach (var part in parts)
            {
                value = get(value, part);
            }
            return value;
        }

        public static IList<string> Keys(JToken token)
        {

            if (token is JArray)
                return Enumerable.Range(0, (token as JArray).Count).Select(x => x.ToString()).ToList();

            if (token is JObject)
                return (token as JObject).Properties().Select(p => p.Name).ToList();

            throw new Exception(String.Format("Invalid token type, expected Array or Object"));
        }
    }
}
