using System;
using System.IO;
using System.Net;
using System.Text;
using GHttpHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GHttpHelperTest
{
    [TestClass]
    public class GHttpTest
    {
        private string Api = "https://s.geekbang.org/api/gksearch/search";
        private string Referer = "https://s.geekbang.org/search/c=2/k=q%E8%B5%84%E8%AE%AF/t=";
        [TestMethod]
        public void TestMethod1()
        {
            var res = Http.Post(Api, new { q = "q资讯", t = 2, s = 20, p = 1 }, RequestType.Json, Referer, Encoding.UTF8);

        }
    }
}
