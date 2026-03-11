using System;
using System.Web ;
using System.Web.SessionState;

namespace Com.ValuePlus.Utils.Session
{
	/// <summary>
	/// SessionHelper 的摘要说明。
	/// </summary>
	public class SessionHelper
	{

#region "构造函数"
		public SessionHelper()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		#endregion

#region "设置seeeion的值"	
		public static void SetSession(string Name,Object Value)
		{
			System.Web.HttpContext.Current.Session[Name] = Value;
		}
		#endregion

#region "获得session的值"
		public static System.Object GetSession(string Name)
		{
			try
			{
				if((System.Web.HttpContext.Current!=null)&&(System.Web.HttpContext.Current.Session!=null))
				{
					return  System.Web.HttpContext.Current.Session[Name];
				}else{
					return null;
                }

			}catch(Exception ex){
				return null;
            }
		}
		#endregion
		
#region "移除session的值"
		public static void RemoveSession(string Name)
		{
			System.Web.HttpContext.Current.Session.Remove(Name);
		}
		#endregion

 #region 禁止当前会话
        public static void AbandonSession()
		{
            System.Web.HttpContext.Current.Session.Abandon();
        }
        #endregion

    }
}
