using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Requests;

namespace Request.Test
{
    class ParserTest
    {
        [Test]
        public void Can_Parse_Top_Level_Array()
        {
            var json = @"[
                {
                    postId: 1,
                    id: 1,
                    name: ""id labore ex et quam laborum"",
                    email: ""Eliseo@gardner.biz"",
                    body: ""laudantium enim quasi est quidem magnam voluptate ipsam eos tempora quo necessitatibus dolor quam autem quasi reiciendis et nam sapiente accusantium""
                },
                {
                    postId: 1,
                    id: 2,
                    name: ""quo vero reiciendis velit similique earum"",
                    email: ""Jayne_Kuhic@sydney.com"",
                    body: ""est natus enim nihil est dolore omnis voluptatem numquam et omnis occaecati quod ullam at voluptatem error expedita pariatur nihil sint nostrum voluptatem reiciendis et""
                }]";
            var token = Parser.Parse(json);
            Assert.IsTrue(token is JArray);
        }
    }
}
