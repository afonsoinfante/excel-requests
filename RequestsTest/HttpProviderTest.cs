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
            var result = httpProvider.Get(url);
        }

        [Test]
        public void Can_Get_Https()
        {
            var url = "https://jsonplaceholder.typicode.com/comments";
            var httpProvider = new HttpProvider();
            var result = httpProvider.Get(url);
        }



    }

}