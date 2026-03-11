<%@ Application Language="C#" %>
<%@ Import Namespace = "System.Data" %>
<%@ Import Namespace = "Com.ValuePlus.DAL" %>
<%@ Import Namespace = "Com.ValuePlus.SysTask" %>
<%@ Import Namespace = "Com.ValuePlus.Utils.Session" %>
<%@ Import Namespace = "Com.ValuePlus.Common" %>
<%@ Import Namespace = "Com.ValuePlus.Common.Config" %>

<script runat="server">
    String strAppStartTime = "";//应用启动时间
    bool isAppStarted = false;//应用是否启动
    bool isSSOLogin = false;//是否多账套单点登录
    String strIsUsingTimer = "1";//是否使用计时器
    String strTimerInterval = "10000";//扫描时间间隔，默认1秒钟
    String strPreMinute = "";//上一分钟数
    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    //定义第一个timer时隔1秒，为了触发业务执行定时器
    System.Timers.Timer firstTimer = new System.Timers.Timer(1000);

    //实例化业务执行定时器Timer类，设置间隔时间默认为1000毫秒，即一秒钟；
    System.Timers.Timer aTimer = new System.Timers.Timer(1000);

    void Application_BeginRequest(object sender, EventArgs e)
    {
        //log.Error("Application_BeginRequest Request.UserLanguages[0] '" + Request.UserLanguages[0].ToString() + "'");
        //设置客户端请求的特殊语言头
        SetUserSpecialLanguage();
    }

    void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        //log.Error("Application_PreRequestHandlerExecute Request.UserLanguages[0] '" + Request.UserLanguages[0].ToString() + "'");
        //设置客户端请求的特殊语言头
        SetUserSpecialLanguage();
    }

    /// <summary>
    /// 设置客户端请求的特殊语言头
    /// </summary>
    private void SetUserSpecialLanguage()
    {
        try
        {
            if (Request.UserLanguages!=null && Request.UserLanguages[0].ToString().ToLower().Equals ("zh-hans-cn"))
            {
                Request.UserLanguages[0] = "zh-cn";
            }
        }
        catch(Exception ex)
        {
            log.Error("Application_PreRequestHandlerExecute Set Language "+Request.UserLanguages[0].ToString()+" Error"+ex.ToString());
        }
    }

    void Application_Start(object sender, EventArgs e)
    {
        ExcuteWhenAppStart();
        //GC.KeepAlive(firstTimer);//不许GC回收firstTimer。
        //GC.KeepAlive(aTimer);//不许GC回收aTimer。
    }

    //void Application_BeginRequest(object sender, EventArgs e)
    //{
    //    //if (!isAppStarted)
    //    //{
    //        log.Error("Application_BeginRequest=============" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    //        System.Diagnostics.Debug.WriteLine("Application_BeginRequest=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    //        ExcuteWhenAppStart();
    //    //}
    //}

    /// <summary>
    /// 应用启动时执行
    /// </summary>
    private void ExcuteWhenAppStart()
    {
        try
        {
            isAppStarted = true;
            strAppStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                isSSOLogin = false;
                //数据库中基础设置(是否启用定时器)
                strIsUsingTimer = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("iIsUsingTimer");
                //数据库中基础设置(定时器时间间隔)----------屏蔽设置，默认1秒钟间隔扫描
                //strTimerInterval = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("iTimerInterval");
                //********在此处获取数据库配置信息，为了可以即时获取数据修改
            }catch(Exception ex)
            {
                isSSOLogin = true;
                //主要是因为多账套单点登录时，无法从Session中获取数据库的相关配置，设置一个默认值
                strIsUsingTimer = "1";
                //strTimerInterval = "60000";//ms;----------屏蔽设置，默认1秒钟间隔扫描
                log.Error("Global.asax中获取基础参数iIsUsingTimer或者iTimerInterval时出错，可能是因为多账套单点登录时，无法从Session中获取数据库的相关配置");
            }

            //在应用程序启动时运行的代码
            firstTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnFirstTimerEvent);
            //启用执行
            firstTimer.Enabled = true;
            log.Error("Application_Start============" + strAppStartTime);

        }
        catch(Exception ex)
        {
            log.Error("Application_Start Error:" + ex);
        }
    }

    /// <summary>
    /// 启动应用的第一次定时器执行，目的是为了调用另一个计时器
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnFirstTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            log.Error("OnFirstTimerEvent=============" + e.SignalTime.ToString("yyyy-MM-dd HH:mm:ss"));
            System.Diagnostics.Debug.WriteLine("OnFirstTimerEvent=" + e.SignalTime.ToString("yyyy-MM-dd HH:mm:ss"));
            if (strIsUsingTimer.Equals("1"))
            {
                ////先执行一遍工作，然后设置定时器
                //DoExecuteSysTask(e.SignalTime);

                //到达时间的时候执行事件； 
                aTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
                //设置间隔时间(毫秒)
                if (!String.IsNullOrEmpty(strTimerInterval))
                {
                    aTimer.Interval = int.Parse(strTimerInterval);
                }
                //设置是执行一次（false）还是一直执行(true)
                aTimer.AutoReset = true;
                //启用执行
                aTimer.Enabled = true;

                firstTimer.Enabled = false;
                //firstTimer.Dispose();
                //firstTimer=null; 
            }

        }
        catch(Exception ex)
        {
            log.Error("Application_Start OnFirstTimerEvent Error:" + ex);
        }

    }

    /// <summary>
    /// 业务定时器执行
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
    {
        //log.Error("OnTimedEvent=============" + strCurTime);
        String strCurTime = e.SignalTime.ToString("yyyy-MM-dd HH:mm:ss");
        String strCurDate = e.SignalTime.ToString("yyyy-MM-dd");
        String strCurHour = e.SignalTime.ToString("HH");
        String strCurMinute = e.SignalTime.ToString("mm");
        //log.Error("strCurTime=============" + strCurTime);
        //log.Error("strPreMinute=============" + strPreMinute);
        //log.Error("strCurMinute=============" + strCurMinute);
        //跨了分钟数的时候才触发扫描时间进行执行
        if (!strCurMinute.Equals(strPreMinute))
        {
            strPreMinute = strCurMinute;
            String strExecuteTime = strCurDate + " " + strCurHour + ":" + strCurMinute + ":00";
            //log.Error("App业务定时器OnTimedEvent整分钟执行=============" + strExecuteTime);

            if (strCurMinute.Equals("00"))
            {
                log.Error("App业务定时器OnTimedEvent整点时执行=============" + strExecuteTime);
            }
            DoExecuteSysTask(DateTime.Parse(strExecuteTime));
        }
    }

    /// <summary>
    /// 执行系统计划任务
    /// </summary>
    /// <param name="dtCurTime"></param>
    private void DoExecuteSysTask(DateTime dtCurTime)
    {
        try
        {
            //如果是单点登录的，从Session获取数据链接字符串的节点标识，如果存在则获取对应的数据库链接（add by sammen 20170901）
            String strDBConnectSessionName = "";
            try
            {
                strDBConnectSessionName = SessionHelper.GetSession(CacheName.DBConnectSessionName) as String;
            }
            catch (Exception ex)
            {

            }

            //非单点登录或者是单点登录时已经存在数据库链接的Session时才执行计划任务
            if((!isSSOLogin)||(isSSOLogin && (!String.IsNullOrEmpty(strDBConnectSessionName))))
            {
                SysTaskBll.ExecuteSysTaskList(dtCurTime);
            }
        }
        catch(Exception ex)
        {
            log.Error("执行计划任务失败:" + ex);
        }
    }


    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码
        //aTimer.Dispose();
        //aTimer = null;

        isAppStarted = false;
        log.Error("Application_End============" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //下面的代码是关键，可解决IIS应用程序池自动回收的问题
        System.Threading.Thread.Sleep(3000);
        //-----这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start
        string url = "";
        try
        {
            //url = String.Format("http://{0}/index.html", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost());
            String strConfigHomePage = "";
            try
            {
                strConfigHomePage = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("WebSiteHostUrl");
            }catch(Exception ex)
            {
                strConfigHomePage = "http://" + Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost();
                log.Error("Global.asax中获取基础参数iIsUsingTimer或者iTimerInterval时出错，可能是因为多账套单点登录时，无法从Session中获取数据库的相关配置");
            }

            url = String.Format("{0}/index.html", strConfigHomePage);
            System.Net.HttpWebRequest myHttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            System.Net.HttpWebResponse myHttpWebResponse = (System.Net.HttpWebResponse)myHttpWebRequest.GetResponse();
            System.IO.Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流
            log.Error("Auto Access Url:(" + url + ")  on time" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            firstTimer.Enabled = true;
            //应用重新开始后的执行
            //ExcuteWhenAppStart();
        }
        catch (Exception ex)
        {
            log.Error("Auto Access Url:(" + url + ") Error" + ex.ToString());
        }

        log.Error("Application_Start After Application_End============" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    void Application_Error(object sender, EventArgs e)
    {
        //在出现未处理的错误时运行的代码

    }

    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。
    }


</script>
