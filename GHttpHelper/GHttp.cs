using CsharpHttpHelper;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WandhiHelper.Extension;

namespace GHttpHelper
{
    public class GHttp
    {
        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static GResult Get(string url, WebHeaderCollection headers = null)
        {
            var http = new HttpItem() { URL = url, IgnoreSecurity = true };
            if (headers != null)
            {
                foreach (var key in headers.AllKeys)
                {
                    http.Header.Add(key, headers[key]);
                }
            }

            var res = new HttpHelper().GetHtml(http);
            return (GResult)res;
        }

        /// <summary>
        /// get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static GResult<T> Get<T>(string url, WebHeaderCollection headers = null) where T : class
        {
            HttpResult httpResult = Get(url, headers);
            var result = (GResult<T>)httpResult;
            result.Data = DeserializeObject<T>(httpResult.Html);
            return result;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data">提交数据</param>
        /// <param name="postType">提交类型</param>
        /// <param name="referer"></param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        private static HttpResult Post(string url, object data, RequestType postType = RequestType.Form,
            string referer = "", Encoding encoding = null, int timeount = 100000)
        {
            // if (postType == RequestType.Json)
            // {
            //     return PostJson(url, JsonConvert.SerializeObject(data), referer, encoding);
            // }

            var item = new HttpItem
            {
                URL = url,
                Method = "POST",
                PostDataType = Enum.PostDataType.String,
                Postdata = GenPara(data),
                ContentType = "application/x-www-form-urlencoded",
                IgnoreSecurity = true,
                Timeout = 100000
            };
            if (!string.IsNullOrEmpty(referer))
            {
                item.Referer = referer;
            }

            if (encoding != null)
            {
                item.PostEncoding = encoding;
            }

            var res = new HttpHelper().GetHtml(item);
            return res;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data">提交数据</param>
        /// <param name="referer"></param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static GResult PostJson(string url, string data, string referer = "", Encoding encoding = null)
        {
            var item = new HttpItem
            {
                URL = url,
                Method = "POST",
                PostDataType = Enum.PostDataType.String,
                Postdata = data,
                IgnoreSecurity = true
            };
            if (!string.IsNullOrEmpty(referer))
            {
                item.Referer = referer;
            }

            if (encoding != null)
            {
                item.PostEncoding = encoding;
            }

            item.ContentType = "application/json";
            var res = new HttpHelper().GetHtml(item);
            return (GResult)res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="postType"></param>
        /// <param name="headers"></param>
        /// <param name="referer"></param>
        /// <param name="encoding"></param>
        /// <param name="timeount"></param>
        /// <returns></returns>
        private static HttpResult Post(string url, object data, RequestType postType = RequestType.Form,
            WebHeaderCollection headers = null, string referer = "", Encoding encoding = null, int timeount = 100000)
        {
            var item = new HttpItem
            {
                URL = url,
                Method = "POST",
                PostDataType = Enum.PostDataType.String,
                Postdata = postType == RequestType.Form ? GenPara(data) : JsonConvert.SerializeObject(data),
                IgnoreSecurity = true,
                Timeout = timeount
            };

            item.ContentType = postType switch
            {
                RequestType.Form => "application/x-www-form-urlencoded",
                RequestType.Json => "application/json",
                _ => item.ContentType
            };

            if (!string.IsNullOrEmpty(referer))
            {
                item.Referer = referer;
            }

            if (encoding != null)
            {
                item.PostEncoding = encoding;
            }

            if (headers != null)
            {
                foreach (var key in headers.AllKeys)
                {
                    item.Header.Add(key, headers[key]);
                }
            }

            var res = new HttpHelper().GetHtml(item);
            return res;
        }

        public static GResult<T> Post<T>(string url, object data, RequestType postType = RequestType.Form,
            string referer = "",
            Encoding encoding = null) where T : class
        {
            var httpResult = Post(url, data, postType, referer, encoding);
            var res = (GResult<T>)httpResult;
            res.Data = DeserializeObject<T>(res.Html);
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="postType"></param>
        /// <param name="headers"></param>
        /// <param name="referer"></param>
        /// <param name="encoding"></param>
        /// <param name="timeout">超时，毫秒</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static GResult<T> Post<T>(string url, object data, RequestType postType = RequestType.Form,
            WebHeaderCollection headers = null, string referer = "", Encoding encoding = null, int timeout = 100000)
            where T : class
        {
            var httpResult = Post(url, data, postType, headers, referer, encoding, timeout);
            var obj = DeserializeObject<T>(httpResult.Html);
            var res = (GResult<T>)httpResult;
            res.Data = obj;
            return res;
        }

        /// <summary>
        /// form参数生成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GenPara(object data)
        {
            var keyValues = data.ToKeyValue();
            var res = string.Join("&",
                keyValues.Select(a => $"{HttpUtility.UrlEncode(a.Key)}={HttpUtility.UrlEncode(a.Value)}"));
            return res;
        }

        private static T DeserializeObject<T>(string str) where T : class
        {
            return JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings
            {
                Error = (sender, args) => { args.ErrorContext.Handled = true; }
            });
        }
    }

    public enum RequestType
    {
        Json = 1,
        Form = 0
    }
}