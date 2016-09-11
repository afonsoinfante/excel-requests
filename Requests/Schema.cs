using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requests
{
    public class Schema
    {
        public string Url { get; private set; }
        public string Base { get; private set; }
        public string Path { get; private set; }


        public static string Trim(string url)
        {
            return url.TrimEnd(new char[] { '/' });
        }


        public Schema(string url)
        {
            Url = Trim(url);
            var baseAndPath = Url.Split('#');
            Base = baseAndPath[0];
            if (baseAndPath.Length == 2)
            {
                Path = baseAndPath[1];
            }
        }
    }
}
