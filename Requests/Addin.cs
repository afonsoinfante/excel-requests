using System;
using System.Collections.Generic;
using ExcelDna.Integration;
using Requests.Providers;
using Newtonsoft.Json.Linq;

namespace Requests
{
    public static class Addin
    {
        private static Cache cache = new Cache();
        private static HttpProvider httpProvider = new HttpProvider();

        [ExcelFunction(Name = "REQUESTS.GET")]
        public static object HttpGet(string url, object query, object headers, object authentication, object timeout, object allowRedirects)
        {
            if (ExcelDnaUtil.IsInFunctionWizard())
                return "";
            try
            {
                url = Schema.Trim(url);

                var httpHeaders = headers is ExcelMissing ?
                    new Dictionary<string, string>() :
                    ExcelParams.AsDictionary<string>(headers as object[,]);

                if(!(authentication is ExcelMissing))
                {
                    var encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(authentication.ToString()));
                    httpHeaders["Authorization"] = "Basic " + encoded;
                }



                if (!cache.ContainsKey(url))
                    cache.Set(url, httpProvider.Get(url, httpHeaders));
                return url;
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }


        /*
        [ExcelFunction(Name = "REQUESTS.PUT")]
        public static object HttpPut(string url, string payload, object headers)
        {
            try
            {
                //dummy
                return url;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        */


        [ExcelFunction(Name = "REQUESTS.FLUSH")]
        public static object Flush(object keys)
        {
            try
            {
                int count = 0;
                if (keys is ExcelMissing)
                {
                    count = cache.Count;
                    cache.Flush();
                }
                else
                {
                    foreach (var key in (keys as string[]))
                    {
                        cache.Flush(key);
                        count++;
                    }
                }
                return String.Format("Flushed {0} key(s)", count);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }



        [ExcelFunction(Name = "REQUESTS.DICT.GET")]
        public static object Get(string key, object property)
        {
            try
            {
                var url = property is ExcelMissing ? key : (key.Contains("#") ? key + "/" + property.ToString() : key + "#" + property.ToString());
                var schema = new Schema(url);
                var token = cache.Get(schema.Base);
                if (schema.Path != null)
                    token = Accessor.Get(token, schema.Path);

                return ExcelRenderer.Render(token, schema.Url, true);
            }
            catch (Exception e)
            {
                return ExcelError.ExcelErrorNA;
            }
        }


        [ExcelFunction(Name = "REQUESTS.DICT.CREATE")]
        public static object Create(string key)
        {
            var token = new JObject();
            cache.Set(key, token);
            return key;
        }



        [ExcelFunction(Name = "REQUESTS.DICT.SET")]
        public static object Set(string key, object property, object value)
        {
            try
            {
                var url = property is ExcelMissing ? key : (key.Contains("#") ? key + "/" + property.ToString() : key + "#" + property.ToString());
                var schema = new Schema(url);
                var token = cache.Get(schema.Base);

                var jValue = new JValue(value);
                if (schema.Path != null)
                    token = Accessor.Set(token, schema.Path, jValue);

                return schema.Url;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }







        [ExcelFunction(Name = "REQUESTS.DICT.KEYS")]
        public static object Keys(string key)
        {
            try
            {
                var schema = new Schema(key);
                var token = cache.Get(schema.Base);
                if (schema.Path != null)
                    token = Accessor.Get(token, schema.Path);

                var properties = Accessor.Properties(token);
                if(properties.Count == 0)
                {
                    return ExcelError.ExcelErrorNA;
                }
                var result = new string[properties.Count, 1];
                for (var i = 0; i < properties.Count; i++)
                    result[i, 0] = properties[i];
                return result;

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }

}
