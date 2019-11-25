using System;

namespace GHttpHelper.Helper
{
    /// <summary>
    /// Json操作对象
    /// </summary>
    internal class JsonHelper
    {
        /// <summary>
        /// 将指定的Json字符串转为指定的T类型对象 
        /// </summary>
        /// <param name="jsonstr">字符串</param>
        /// <returns>转换后的对象，失败为Null</returns>
        internal static object JsonToObject<T>(string jsonstr)
        {
            try
            {                
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonstr);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 将指定的对象转为Json字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>转换后的字符串失败为空字符串</returns>
        internal static string ObjectToJson(object obj)
        {
            try
            {                
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj); ;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
