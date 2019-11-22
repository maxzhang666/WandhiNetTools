using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WandhiHelper.libs
{
    class WinApi
    {
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        const int LOCALE_SLONGDATE = 0x20;
        const int LOCALE_SSHORTDATE = 0x1F;
        const int LOCALE_STIME = 0x1003;

        [DllImport("kernel32.dll", EntryPoint = "GetSystemDefaultLCID")]
        static extern int GetSystemDefaultLCID();
        [DllImport("kernel32.dll", EntryPoint = "SetLocaleInfoA")]
        static extern int SetLocaleInfo(int Locale, int LCType, string lpLCData);
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        /// <summary>
        /// 设置系统日期格式
        /// </summary>
        public static void SetDateTimeFormat()
        {
            try
            {
                int x = GetSystemDefaultLCID();
                SetLocaleInfo(x, LOCALE_STIME, "HH:mm:ss");        //时间格式  
                SetLocaleInfo(x, LOCALE_SSHORTDATE, "yyyy-MM-dd");   //短日期格式    
                SetLocaleInfo(x, LOCALE_SLONGDATE, "yyyy-MM-dd");   //长日期格式   
            }
            catch
            {
            }
        }
    }
}
