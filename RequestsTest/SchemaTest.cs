using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Requests;

namespace Request.Test
{
    class SchemaTest
    {
        [Test]
        public void Can_Parse_Url_Without_Hash()
        {
            var url = "http://api.test.com/";
            var schema = new Schema(url);
            Assert.AreEqual("http://api.test.com", schema.Base); //removes trailing slash
            Assert.IsNull(schema.Path);
        }


        [Test]
        public void Can_Parse_Url_With_Hash()
        {
            var url = "http://api.test.com#path/to/thingy";
            var schema = new Schema(url);
            Assert.AreEqual("http://api.test.com", schema.Base);
            Assert.AreEqual(schema.Path, "path/to/thingy");
        }

    }
}
