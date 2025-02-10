namespace GHttpHelper.Axios.Interceptors
{
    public interface IRequestInterceptor
    {
        HttpItem OnRequest(HttpItem config);
    }
}