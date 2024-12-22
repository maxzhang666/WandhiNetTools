namespace GHttpHelper;

public class GResult<T> : HttpResult where T : class
{
    public T Data { get; set; }
}

public class GResult : GResult<string>
{
}