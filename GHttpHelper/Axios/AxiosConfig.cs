using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GHttpHelper.Axios
{
    public class AxiosConfig
    {
        /// <summary>
        /// 基础URL
        /// </summary>
        public string BaseUrl { get; set; }
        
        /// <summary>
        /// 超时时间(毫秒)
        /// </summary>
        public int Timeout { get; set; } = 100000;
        
        /// <summary>
        /// 请求头
        /// </summary>
        public WebHeaderCollection Headers { get; set; } = new WebHeaderCollection();
        
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        
        /// <summary>
        /// 是否自动处理Cookie
        /// </summary>
        public bool WithCredentials { get; set; } = false;
    }
}