using System;

namespace GHttpHelper.Axios
{
    internal class CacheItem
    {
        public object Data { get; }
        public DateTime ExpirationTime { get; }
        
        public bool IsExpired => DateTime.Now > ExpirationTime;

        public CacheItem(object data, DateTime expirationTime)
        {
            Data = data;
            ExpirationTime = expirationTime;
        }
    }
}