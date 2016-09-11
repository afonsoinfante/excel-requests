using System;
using System.Collections.Generic;
using ExcelDna.Integration;
using Requests.Providers;

namespace Requests
{
    public static class Addin
    {
        private static Cache cache = new Cache();
        private static HttpProvider httpProvider = new HttpProvider();

        [ExcelFunction(Name = "REQUESTS.GET")]
        public static object HttpGet(string url, object headers)
        {
            try
            {
                url = Schema.Trim(url);
                var httpHeaders = headers is ExcelMissing ?
                    new Dictionary<string, string>() :
                    ExcelParams.AsDictionary<string>(headers as object[,]);

                if (!cache.ContainsKey(url))
                    cache.Set(url, httpProvider.Get(url, httpHeaders));
                return url;
            }
            catch(Exception e)
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
                return e.Message;
            }
        }





        [ExcelFunction(Name = "REQUESTS.DICT.KEYS")]
        public static object Properties(string key)
        {
            try
            {
                var schema = new Schema(key);
                var token = cache.Get(schema.Base);
                if (schema.Path != null)
                    token = Accessor.Get(token, schema.Path);

                var properties = Accessor.Properties(token);
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
