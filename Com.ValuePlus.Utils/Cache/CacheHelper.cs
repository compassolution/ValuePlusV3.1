using System;
using System.Web;

namespace Com.ValuePlus.Utils.Cache
{
	/// <summary>
	/// CacheHelper 的摘要说明。
	/// </summary>
	public class CacheHelper
	{

#region "构造函数"
		public CacheHelper()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		#endregion

#region "设置cache的值"
		public static void SetCache(string Name,Object Value)
		{
			System.Web.HttpContext.Current.Cache[Name] = Value;
		}
		#endregion

#region "获得cache的值"
		public static System.Object GetCache(string Name)
		{
			return System.Web.HttpContext.Current.Cache[Name];
		}
		#endregion

#region "移除cache的值"
		public static void RemoveCache(string Name)
		{
			System.Web.HttpContext.Current.Cache.Remove(Name);
		}
		#endregion

#region "设置cache的值采用httpruntime"
		public static void SetCacheRuntime(string Name,Object Value)
		{
			System.Web.HttpRuntime.Cache[Name] = Value;
		}
		#endregion

#region "获得cache的值采用httpruntime"
		public static System.Object GetCacheRuntime(string Name)
		{
			return System.Web.HttpRuntime.Cache[Name];
		}
		#endregion

#region "移除cache的值"
		public static void RemoveCacheRuntime(string Name)
		{
			System.Web.HttpRuntime.Cache.Remove(Name);
		}
		#endregion

	}
}
