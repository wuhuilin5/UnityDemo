using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityDemo.Utils
{
    public class TimeUtils
    {
        private static DateTime timeStamp = new DateTime(1971, 1, 1);

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public static int GetTime()
        {
            return Convert.ToInt32((DateTime.UtcNow.Ticks - timeStamp.Ticks) / 1.0e7);
        }
    }
}

