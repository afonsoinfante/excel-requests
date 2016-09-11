using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using Requests.Providers;

namespace Requests
{
    public class Cache
    {
        private Dictionary<string, JToken> objects = new Dictionary<string, JToken>();
        private HttpProvider httpProvider = new HttpProvider();

        public JToken Fetch(string url)
        {
            var json = httpProvider.Get(url);
            return Parse(json);
        }

        public JToken Parse(string json)
        {
            if(json.StartsWith("["))
                return JArray.Parse(json);

            return JObject.Parse(json);
        }

        public bool ContainsKey(string key)
        {
            return objects.ContainsKey(key);
        }


        public JToken Get(string key)
        {
            if(!objects.ContainsKey(key))
            {
                objects[key] = Fetch(key);
            }

            return objects[key];
        }

        public JToken Set(string key, JToken token)
        {
            objects[key] = token;
            return token;
        }


    }
}
