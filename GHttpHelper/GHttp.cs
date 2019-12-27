using CsharpHttpHelper;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Web;
using WandhiHelper.Extension;

namespace GHttpHelper
{
    public class Http
    {
        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            var http = new HttpItem()
            {
                URL = url
            };
            var res = new HttpHelper().GetHtml(http);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return res.Html;
            }
            return "";
        }
        /// <summary>
        /// get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T Get<T>(string url)
        {
            var res = Get(url);
            var obj = JsonConvert.DeserializeObject<T>(res);
            return obj;
        }
        public static string Post(string url, object data, RequestType postType = RequestType.Form, string Referer = "", Encoding Encoding = null)
        {
            var item = new HttpItem
            {
                URL = url,
                Method = "POST",
                PostDataType = Enum.PostDataType.String,
                Postdata = postType == RequestType.Form ? GenPara(data) : JsonConvert.SerializeObject(data)
            };
            if (postType == RequestType.Form)
            {
                item.ContentType = "application/x-www-form-urlencoded";
            }
            if (!string.IsNullOrEmpty(Referer))
            {
                item.Referer = Referer;
            }
            if (Encoding != null)
            {
                item.PostEncoding = Encoding;
            }
            var res = new HttpHelper().GetHtml(item);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return res.Html;
            }
            else
            {
                throw new Exception($"请求异常:{res.StatusCode}") { Source = JsonConvert.SerializeObject(res) };
            }
        }
        public static T Post<T>(string url, object data, RequestType postType = RequestType.Form, string Referer = "", Encoding Encoding = null)
        {
            var res = Post(url, data, postType, Referer, Encoding);
            var obj = JsonConvert.DeserializeObject<T>(res);
            return obj;
        }
        /// <summary>
        /// form参数生成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GenPara(object data)
        {
            var KeyValues = data.ToKeyValue();
            var res = string.Join("&", KeyValues.Select(a => $"{HttpUtility.UrlEncode(a.Key)}={HttpUtility.UrlEncode(a.Value)}"));
            return res;
        }
    }
    public enum RequestType
    {
        Json = 1,
        Form = 0
    }
}