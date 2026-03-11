using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;

namespace Com.ValuePlus.Common.Exception
{
    public class ExceptionInfo
    {

        /// <summary>
        /// 获取异常提示信息
        /// </summary>        
        /// <param name="TipName"></param>
        /// <returns></returns>
        public static string GetExceptionTipInfo(string sTipName)
        {          
            //获取异常语句提示
            ResourceManager sR = new ResourceManager("Resources.ExceptionTip", Assembly.Load("App_GlobalResources"));
            return sR.GetString(sTipName);
        }
    }
}
