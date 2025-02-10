# GHttpHelper

一个基于 .NET 的 HTTP 请求库，提供类似 Axios 的 API 和功能。

## 特性

- 支持常用的 HTTP 方法（GET、POST、PUT、DELETE、PATCH）
- 请求/响应拦截器
- 自动重试机制
- 请求并发控制
- GET 请求缓存
- 支持 Form 和 JSON 格式数据
- 可配置的请求超时
- 支持取消请求
- 数据转换器

## 安装

```bash
dotnet add package GHttpHelper
```

## 基础使用

### 创建实例

```csharp
var axios = new GAxios(new AxiosConfig 
{
    BaseUrl = "https://api.example.com",
    Timeout = 5000
});
```

### GET 请求

```csharp
// 基础 GET 请求
var result = await axios.Get<UserInfo>("/users/1");

// 带请求头的 GET 请求
var headers = new WebHeaderCollection();
headers.Add("Authorization", "Bearer token");
var result = await axios.Get<UserInfo>("/users/1", headers);
```

### POST 请求

```csharp
// Form 格式提交
var formResult = await axios.Post<ResponseData>("/api/submit", new { 
    name = "test",
    age = 18
});

// JSON 格式提交
var jsonResult = await axios.Post<ResponseData>("/api/submit", new { 
    name = "test",
    age = 18
}, RequestType.Json);
```

### PUT 和 DELETE 请求

```csharp
// PUT 请求
var putResult = await axios.Put<ResponseData>("/api/users/1", new { 
    name = "updated"
});

// DELETE 请求
var deleteResult = await axios.Delete<ResponseData>("/api/users/1");
```

## 高级特性

### 拦截器

```csharp
// 添加请求拦截器
axios.RequestInterceptors.Use(config => 
{
    config.Header.Add("Authorization", "Bearer token");
    Console.WriteLine("请求发送前");
});

// 添加响应拦截器
axios.ResponseInterceptors.Use(
    response => 
    {
        Console.WriteLine("响应接收后");
    },
    error => 
    {
        Console.WriteLine($"请求错误：{error.Message}");
    }
);
```

### 重试机制

```csharp
axios.RetryConfig = new RetryConfig 
{
    RetryCount = 3,
    RetryDelay = 1000,
    RetryCondition = ex => ex is HttpRequestException
};
```

### 并发控制

```csharp
// 设置最大并发请求数
axios.MaxConcurrentRequests = 5;
```

### 缓存控制

```csharp
// 设置 GET 请求缓存时间
axios.CacheDuration = TimeSpan.FromMinutes(10);
```

### 创建新实例

```csharp
var newAxios = axios.Create(new AxiosConfig 
{
    BaseUrl = "https://api2.example.com",
    Timeout = 5000
});
```

### 数据转换

```csharp
// 请求数据转换
axios.TransformRequest = data => 
{
    return JsonConvert.SerializeObject(data, new JsonSerializerSettings 
    {
        NullValueHandling = NullValueHandling.Ignore
    });
};

// 响应数据转换
axios.TransformResponse = (data, type) => 
{
    return JsonConvert.DeserializeObject(data, type, new JsonSerializerSettings 
    {
        DateTimeZoneHandling = DateTimeZoneHandling.Local
    });
};
```

### 统一请求配置

```csharp
var result = await axios.Request<ResponseData>(new AxiosRequestConfig 
{
    Url = "/api/data",
    Method = "POST",
    Data = new { id = 1 },
    RequestType = RequestType.Json,
    Headers = new WebHeaderCollection()
});
```

## 错误处理

```csharp
try 
{
    var result = await axios.Get<UserInfo>("/users/1");
} 
catch (Exception ex) 
{
    Console.WriteLine($"请求失败：{ex.Message}");
}
```

## 注意事项

1. GET 请求的缓存仅在内存中保存
2. 重试机制默认对所有异常生效，可通过 RetryCondition 自定义重试条件
3. 并发控制会对所有请求生效，包括重试的请求
4. 拦截器按添加顺序执行

## 许可证

MIT License