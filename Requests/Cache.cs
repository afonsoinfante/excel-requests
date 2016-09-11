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

        public bool ContainsKey(string key)
        {
            return objects.ContainsKey(key);
        }


        public JToken Get(string key)
        {
            return objects[key];
        }

        public JToken Set(string key, JToken token)
        {
            objects[key] = token;
            return token;
        }


    }
}
