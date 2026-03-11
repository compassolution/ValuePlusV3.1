<%@ WebHandler Language="C#" Class="Archive" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.Property;
using Com.ValuePlus.SysParams;
using Com.ValuePlus.Common.Security;

public class Archive : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);

        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strUserId = context.Request["userid"] == null ? string.Empty : context.Request["userid"].ToString();//登录名
        string strTID = context.Request["tid"] == null ? string.Empty : context.Request["tid"].ToString();//TID
        string strRID = context.Request["rid"] == null ? string.Empty : context.Request["rid"].ToString();//RID
        string strSID = context.Request["sid"] == null ? string.Empty : context.Request["sid"].ToString();//SID
        string strGID = context.Request["gid"] == null ? string.Empty : context.Request["gid"].ToString();//SID
        string strKEY = context.Request["key"] == null ? string.Empty : context.Request["key"].ToString();//key
        string strKEYVALUE = context.Request["keyvalue"] == null ? string.Empty : context.Request["keyvalue"].ToString();//keyvalue
        string strGRIDKEY = context.Request["gridkey"] == null ? string.Empty : context.Request["gridkey"].ToString();//key
        string strGRIDKEYVALUE = context.Request["gridkeyvalue"] == null ? string.Empty : context.Request["gridkeyvalue"].ToString();//keyvalue
        string strIsInsert = context.Request["isinsert"] == null ? string.Empty : context.Request["isinsert"].ToString();//isinsert
        string strSaveData = context.Request["savedata"] == null ? string.Empty : context.Request["savedata"].ToString();//savedata
        string strSql = context.Request["sql"] == null ? string.Empty : context.Request["sql"].ToString();//sql
        string strCondition = context.Request["condition"] == null ? string.Empty : context.Request["condition"].ToString();//condition
        string strRequestLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//终端请求时的语言

        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strUserId = SQLInjectionDefense.ReplaceSQLReservedKeyword(strUserId);
        strTID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strTID);
        strRID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRID);
        strSID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSID);
        strGID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strGID);
        strKEY = SQLInjectionDefense.ReplaceSQLReservedKeyword(strKEY);
        strKEYVALUE = SQLInjectionDefense.ReplaceSQLReservedKeyword(strKEYVALUE);
        strGRIDKEY = SQLInjectionDefense.ReplaceSQLReservedKeyword(strGRIDKEY);
        strGRIDKEYVALUE = SQLInjectionDefense.ReplaceSQLReservedKeyword(strGRIDKEYVALUE);
        strIsInsert = SQLInjectionDefense.ReplaceSQLReservedKeyword(strIsInsert);
        strSaveData = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSaveData);
        //strSql = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSql);//暂时不替换
        strCondition = SQLInjectionDefense.ReplaceSQLReservedKeyword(strCondition);
        strRequestLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestLanguage);

        //log.Error("本次请求：param="+param+";LANGUAGE=" + strRequestLanguage+";UserId="+strUserId);
        //log.Error("本次请求：TID=" + strTID+";RID=" + strRID+";SID=" + strSID+"；strGID="+strGID+";strKEY=" + strKEY+";strKEYVALUE=" + strKEYVALUE);
        MobileArchive mobileArchive = new MobileArchive();
        switch (param.ToLower().ToString())
        {
            case "archivelist"://获取模板主要列表
                context.Response.Write(mobileArchive.GetArchiveListPageData(strUserId,strTID,strRID,strSID,strCondition,strRequestLanguage));
                break;
            case "archivedetailmain"://获取模板相关信息以及明细主信息表相关信息，列表型分组明细页面的获取
                context.Response.Write(mobileArchive.GetArchiveDetailMain(strUserId,strTID,strRID,strSID,strGID,strKEY,strKEYVALUE,strGRIDKEY,strGRIDKEYVALUE,strRequestLanguage));
                break;
            case "onegrouppropertyanddata"://获取模板某分组的字段配置及业务数据
                context.Response.Write(mobileArchive.GetOneGroupDetailData(strUserId,strTID,strRID,strSID,strGID,strKEY,strKEYVALUE,strGRIDKEY,strGRIDKEYVALUE,strRequestLanguage));
                break;
            //case "onegridgroupdatalist"://获取模板列表型分组的业务数据
            //    context.Response.Write(mobileArchive.GetOneListGroupListData(strUserId,strTID,strRID,strSID,strGID,strKEY,strKEYVALUE,strRequestLanguage));
            //    break;
            case "detaillocationaction"://获取模板明细页面中需要加载的动作列表
                //context.Response.Write(mobileArchive.GetArchiveActionData(strUserId,strTID,strRID,strSID,"","'1'",strRequestLanguage));
                break;
            case "deletedatalist"://删除模板主页数据
                //context.Response.Write(mobileArchive.DeleteArchiveListData(strUserId,strTID,strRID,strSID,strGID,strKEY,strKEYVALUE));
                break;
            case "dobeforeloadscene"://执行需要再加载场景前的逻辑
                int iReturn1 = mobileArchive.DoExcuteBeforeLoadSence(strTID,strRID,strSID,strUserId);
                context.Response.Write(iReturn1.ToString());
                break;
            case "dobeforeloaddetail"://执行需要再加载明细页面前的逻辑
                int iReturn2 = mobileArchive.DoExcuteBeforeLoadDetail(strTID,strRID,strSID,strKEY,strKEYVALUE,strUserId);
                context.Response.Write(iReturn2.ToString());
                break;
            case "getdatabysql"://通过SQL获取数据jason
                context.Response.Write(mobileArchive.GetDataBySQL(strSql,"",strRequestLanguage));
                break;
            case "savedetail"://保存模板明细数据(包含多个group同时保存)
                context.Response.Write(mobileArchive.SaveArchiveDetailData(strUserId,strTID,strRID,strSID,strKEY,strKEYVALUE,strGRIDKEY,strGRIDKEYVALUE,strIsInsert,strSaveData,strRequestLanguage));
                break;
            default:
                break;
        }
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}