// See https://aka.ms/new-console-template for more information

using System.Net;
using GHttpHelper;
using Newtonsoft.Json;

var head = new WebHeaderCollection { { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36" } };
var html = GHttp.Get("https://cloud.189.cn/api/open/share/getShareInfoByCodeV2.action?noCache=0.988092137406307&shareCode=mQbmqeQRvEzy",new WebHeaderCollection()
{
    { HttpRequestHeader.Accept,"application/json;charset=UTF-8"},
    // { HttpRequestHeader.ContentType,"application/json;charset=UTF-8" }
});
// var html = GHttp.Get("https://1024tools.com/header", head);
// var html = GHttp.Get("https://useragent.buyaocha.com", head);

// var urls = PanCrawl.GetPanUrls(html);


// var res = GHttp.PostJson("https://pan.wandhi.com/inner_api/addTask",
// JsonConvert.SerializeObject(new
// {
// url = "https://pan.quark.cn/s/f9999c7c1992,https://pan.quark.cn/s/a70a8a1d97cc,https://pan.quark.cn/s/d1c343a8f852,https://pan.quark.cn/s/7affa4f1b656,https://pan.quark.cn/s/108523a78537,https://pan.quark.cn/s/77b2e9652207,https://pan.quark.cn/s/47bfa6e42ce0"
// }));

Console.WriteLine("Hello, World!");