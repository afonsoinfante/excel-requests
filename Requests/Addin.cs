using System;
using ExcelDna.Integration;
using Requests.Providers;

namespace Requests
{
    public static class Addin
    {
        private static Cache cache = new Cache();
        private static HttpProvider httpProvider = new HttpProvider();

        [ExcelFunction(Name = "REQUESTS.GET")]
        public static object Get(string url)
        {
            try
            {
                var schema = new Schema(url);
                var token = cache.ContainsKey(schema.Base) ? cache.Get(url) : cache.Set(url, Parser.Parse(httpProvider.Get(url)));
                if (schema.Path != null)
                    token = Accessor.Get(token, schema.Path);

                return ExcelRenderer.Render(token, schema.Url, true);
            }
            catch(Exception e)
            {
                return e.Message;
            }

        }


        [ExcelFunction(Name = "REQUESTS.KEYS")]
        public static object Keys(string url)
        {
            try
            {
                var schema = new Schema(url);
                var token = cache.ContainsKey(schema.Base) ? cache.Get(url) : cache.Set(url, Parser.Parse(httpProvider.Get(url)));
                if (schema.Path != null)
                    token = Accessor.Get(token, schema.Path);

                var keys = Accessor.Keys(token);
                var result = new string[keys.Count, 1];
                for (var i = 0; i < keys.Count; i++)
                    result[i, 0] = keys[i];
                return result;

            }
            catch (Exception e)
            {
                return e.Message;
            }

        }



    }

}
