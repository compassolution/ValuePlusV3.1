using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using System.Resources;
using Com.ValuePlus.DAL;

public partial class Welcome : Com.ValuePlus.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("Welcome");

            //从基本参数配置中读取中英文的欢迎词
            String strWelcomeSpeech = "";
            if (this.Language.Equals("zh-cn"))
            {
                strWelcomeSpeech = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("WelcomeSpeech_Chs");
            }
            else
            {
                strWelcomeSpeech = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("WelcomeSpeech_Eng");
            }

            if (String.IsNullOrEmpty(strWelcomeSpeech))
            {
                this.spanTitle.InnerText = rmLocResourceManager.GetString("lbWelcomeTitle").ToString();
                log.Error("Welcome.Page_Load() error：参数配置中不存在WelcomeSpeech_Chs或者WelcomeSpeech_Eng的配置项目!");
            }
            else
            {
                this.spanTitle.InnerText = strWelcomeSpeech;
            }


            this.spanSec_Pending.InnerText = rmLocResourceManager.GetString("lbSec_Pending").ToString();
            this.spanSec_Notice.InnerText = rmLocResourceManager.GetString("lbSec_Notice").ToString();
            //this.spanSec_CopyRight.InnerText = rmLocResourceManager.GetString("lbSec_CopyRight").ToString();

        }
    }

    /// <summary>
    /// 获取HR系统的期间信息【暂不实现】
    /// </summary>
    private void GetHRPeriodInfo(){
        //this.trHRPeriod.Visible = false;
        try
        {
            String strSql_KQ = "select TOP 1 PID,CONVERT(VARCHAR(20),PSTART) AS PSTART,CONVERT(VARCHAR(20),PEND) AS PEND from KQPERD_1 WHERE PKQISNOW = '1' ORDER BY PID DESC ";
            String strSql_XZ = "select TOP 1 PID,CONVERT(VARCHAR(20),PSTART) AS PSTART,CONVERT(VARCHAR(20),PEND) AS PEND from KQPERD_1 WHERE PISNOW = '1' ORDER BY PID DESC ";
            DataTable dt_KQ = SqlParamDao.GetDataTableBySql(strSql_KQ);
            DataTable dt_XZ = SqlParamDao.GetDataTableBySql(strSql_XZ);
            if(dt_KQ.Rows.Count==1 || dt_XZ.Rows.Count==1)
            {
                //this.trHRPeriod.Visible = true;

            }
        }
        catch(Exception ex){
            
        }
    }
}
