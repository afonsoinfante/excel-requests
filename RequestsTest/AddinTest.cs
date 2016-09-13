using NUnit.Framework;
using ExcelDna.Integration;
using Requests;

namespace Request.Test
{
    class AddinTest
    {
        [Test]
        public void Can_Create_Dictionary()
        {
            var key = "test";
            Assert.AreEqual(key, Addin.Create(key));
            Assert.AreEqual(ExcelError.ExcelErrorNA, Addin.Keys(key));
        }


        [Test]
        public void Can_Set_Dictionary()
        {
            var key = "test";
            Addin.Create(key);
            Assert.AreEqual("test#key1", Addin.Set(key, "key1", 12.1));
            Assert.AreEqual(12.1, Addin.Get(key, "key1"));
        }


        [Test]
        public void Can_Set_Nested_Dictionary()
        {
            var key = "test";
            Addin.Create(key);
            Assert.AreEqual("test#key1/key2", Addin.Set(key, "key1/key2", 12.1));
            Assert.AreEqual(12.1, Addin.Get(key, "key1/key2"));
        }

        [Test]
        public void Can_Post()
        {
            var url = "https://httpbin.org/post";
            Addin.Create("empty");
            Assert.AreEqual(url, Addin.HttpPost(url, ExcelMissing.Value, "empty", 
                ExcelMissing.Value, ExcelMissing.Value, ExcelMissing.Value, ExcelMissing.Value));
        }

        



    }
}
