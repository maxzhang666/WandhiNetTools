using System;

namespace GHttpHelper.Axios.Interceptors
{
    public interface IResponseInterceptor
    {
        HttpResult OnResponse(HttpResult response);
        void OnError(Exception error);
    }
}