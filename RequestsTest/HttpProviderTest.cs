using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Requests.Providers;


namespace Requests.Test
{

    public class HttpProviderTest
    {


        [Test]
        public void Can_Get_Http()
        {
            var url = "http://ip.jsontest.com/";
            var httpProvider = new HttpProvider();
            var result = httpProvider.Get(url, new Dictionary<string, string>());
            Assert.AreEqual(200, result.Value<int>("StatusCode"));
        }

        [Test]
        public void Can_Get_Https()
        {
            var url = "https://jsonplaceholder.typicode.com/comments";
            var httpProvider = new HttpProvider();
            var result = httpProvider.Get(url, new Dictionary<string, string>());
            Assert.AreEqual(200, result.Value<int>("StatusCode"));
        }
    }
}