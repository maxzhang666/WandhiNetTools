using System;

namespace GHttpHelper.Axios
{
    public class RetryConfig
    {
        public int RetryCount { get; set; } = 3;
        public int RetryDelay { get; set; } = 1000;
        public Func<Exception, bool> RetryCondition { get; set; }
    }
}