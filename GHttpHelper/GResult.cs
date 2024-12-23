using System;
using System.Net;
using Newtonsoft.Json;

namespace GHttpHelper;

public class GResult<T> where T : class
{
    public GResult()
    {
    }

    public GResult(HttpResult httpResult)
    {
        Code = httpResult.StatusCode;
        Html = httpResult.Html;
        if (Type.GetTypeCode(typeof(T)) == TypeCode.String)
        {
            Data = Html as T;
        }
        else
        {
            Data = DeserializeObject<T>(httpResult.Html);
        }
    }

    public HttpStatusCode Code { get; set; }
    public string Html { get; set; }
    public T Data { get; set; }

    private static T DeserializeObject<T>(string str) where T : class
    {
        return JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings
        {
            Error = (sender, args) => { args.ErrorContext.Handled = true; }
        });
    }
}

public class GResult : GResult<string>
{
    public GResult(HttpResult httpResult) : base(httpResult)
    {
    }
}