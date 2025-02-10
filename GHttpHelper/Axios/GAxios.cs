using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WandhiHelper.Extension;
using GHttpHelper.Axios.Interceptors;
using System.Collections.Concurrent;
using System.Threading;

namespace GHttpHelper.Axios
{
    public class GAxios
    {
        private readonly AxiosConfig _config;
        private readonly HttpHelper _httpHelper;

        public InterceptorManager<HttpItem> RequestInterceptors { get; }
        public InterceptorManager<HttpResult> ResponseInterceptors { get; }

        private readonly SemaphoreSlim _semaphore;
        private readonly ConcurrentDictionary<string, CacheItem> _cache;
        
        public RetryConfig RetryConfig { get; set; }
        public int MaxConcurrentRequests { get; set; } = 10;
        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromMinutes(5);

        public GAxios(AxiosConfig config = null)
        {
            _config = config ?? new AxiosConfig();
            _httpHelper = new HttpHelper();
            RequestInterceptors = new InterceptorManager<HttpItem>();
            ResponseInterceptors = new InterceptorManager<HttpResult>();
            _semaphore = new SemaphoreSlim(MaxConcurrentRequests);
            _cache = new ConcurrentDictionary<string, CacheItem>();
        }

        private async Task<GResult<T>> SendRequest<T>(string url, string method, object data, 
            RequestType requestType = RequestType.Form, WebHeaderCollection headers = null) where T : class
        {
            // 检查缓存
            var cacheKey = GenerateCacheKey(url, method, data);
            if (method.Equals("GET", StringComparison.OrdinalIgnoreCase) && 
                _cache.TryGetValue(cacheKey, out var cacheItem) && 
                !cacheItem.IsExpired)
            {
                return cacheItem.Data as GResult<T>;
            }

            await _semaphore.WaitAsync();
            try
            {
                var retryCount = 0;
                while (true)
                {
                    try
                    {
                        var httpItem = new HttpItem
                        {
                            URL = CombineUrl(_config.BaseUrl, url),
                            Method = method,
                            Timeout = _config.Timeout,
                            Header = headers ?? _config.Headers,
                            PostEncoding = _config.Encoding,
                            IgnoreSecurity = true
                        };

                        if (data != null)
                        {
                            httpItem.PostDataType = Enum.PostDataType.String;
                            httpItem.ContentType = requestType == RequestType.Form ? 
                                "application/x-www-form-urlencoded" : 
                                "application/json";
                            httpItem.Postdata = requestType == RequestType.Form ? 
                                GenPara(data) : 
                                JsonConvert.SerializeObject(data);
                        }

                        RequestInterceptors.ExecuteHandlers(httpItem);
                        var response = _httpHelper.GetHtml(httpItem);
                        ResponseInterceptors.ExecuteHandlers(response);
                        var result = new GResult<T>(response);

                        // 缓存 GET 请求结果
                        if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                        {
                            _cache.AddOrUpdate(cacheKey, 
                                new CacheItem(result, DateTime.Now.Add(CacheDuration)),
                                (_, __) => new CacheItem(result, DateTime.Now.Add(CacheDuration)));
                        }

                        return result;
                    }
                    catch (Exception ex)
                    {
                        ResponseInterceptors.ExecuteErrorHandlers(ex);
                        
                        if (RetryConfig != null && 
                            retryCount < RetryConfig.RetryCount && 
                            (RetryConfig.RetryCondition?.Invoke(ex) ?? true))
                        {
                            retryCount++;
                            await Task.Delay(RetryConfig.RetryDelay);
                            continue;
                        }
                        
                        throw;
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private string GenerateCacheKey(string url, string method, object data)
        {
            var key = $"{method}:{url}";
            if (data != null)
            {
                key += $":{JsonConvert.SerializeObject(data)}";
            }
            return key;
        }

        public async Task<GResult<T>> Get<T>(string url, WebHeaderCollection headers = null) where T : class
        {
            var httpItem = new HttpItem
            {
                URL = CombineUrl(_config.BaseUrl, url),
                Method = "GET",
                Timeout = _config.Timeout,
                Header = headers ?? _config.Headers,
                PostEncoding = _config.Encoding,
                IgnoreSecurity = true
            };

            try 
            {
                // 执行请求拦截器
                RequestInterceptors.ExecuteHandlers(httpItem);

                var response = _httpHelper.GetHtml(httpItem);
                
                // 执行响应拦截器
                ResponseInterceptors.ExecuteHandlers(response);

                return new GResult<T>(response);
            }
            catch (Exception ex)
            {
                ResponseInterceptors.ExecuteErrorHandlers(ex);
                throw;
            }
        }

        public async Task<GResult<T>> Put<T>(string url, object data, RequestType requestType = RequestType.Json, 
            WebHeaderCollection headers = null) where T : class
        {
            return await SendRequest<T>(url, "PUT", data, requestType, headers);
        }

        public async Task<GResult<T>> Delete<T>(string url, WebHeaderCollection headers = null) where T : class
        {
            return await SendRequest<T>(url, "DELETE", null, RequestType.Json, headers);
        }

        public async Task<GResult<T>> Patch<T>(string url, object data, RequestType requestType = RequestType.Json, 
            WebHeaderCollection headers = null) where T : class
        {
            return await SendRequest<T>(url, "PATCH", data, requestType, headers);
        }

        public async Task<GResult<T>> Post<T>(string url, object data, RequestType requestType = RequestType.Form, 
            WebHeaderCollection headers = null) where T : class
        {
            return await SendRequest<T>(url, "POST", data, requestType, headers);
        }

        private string GenPara(object data)
        {
            if (data == null) return string.Empty;
            
            var keyValues = data.ToKeyValue();
            return string.Join("&",
                keyValues.Select(a => $"{HttpUtility.UrlEncode(a.Key)}={HttpUtility.UrlEncode(a.Value)}"));
        }

        private string CombineUrl(string baseUrl, string url)
        {
            if (string.IsNullOrEmpty(baseUrl)) return url;
            
            baseUrl = baseUrl.TrimEnd('/');
            url = url.TrimStart('/');
            
            return $"{baseUrl}/{url}";
        }
    }
}