using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requests.Providers
{
    public class HttpProvider
    {
        public JToken Get(string url, Dictionary<string, string> headers)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            var response = new JObject();
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.Expect100Continue = false;
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);
            using (var httpResponse = request.GetResponse() as HttpWebResponse)
            {
                response.Add("Text", JValue.CreateNull());
                response.Add("StatusCode", (int)httpResponse.StatusCode);
                response.Add("StatusDescription", httpResponse.StatusDescription);
                response.Add("ContentType", httpResponse.ContentType);
                response.Add("Method", httpResponse.Method);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var reader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
                    {
                        var text = reader.ReadToEnd();
                        response["Text"] = text;
                        if(httpResponse.ContentType.ToLower().Contains("application/json"))
                            response.Add("Json", Parser.Parse(text));
                    }
                }
            }
            return response;
        }
    }
}
