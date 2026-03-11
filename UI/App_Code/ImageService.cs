using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.IO;

/// <summary>
///ImageService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class ImageService : System.Web.Services.WebService
{

    public ImageService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
    /// <summary>
    /// 获取首页图片
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public ArrayList getHomeImages()
    {
        return this.getImages("../Res/HomeImage", "getHomeImages");
    }

    /// <summary>
    /// 获取酒店介绍图片
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public ArrayList getHotelImages()
    {
        //ArrayList arrList = new ArrayList();

        //arrList.Add("http://192.168.1.101/VP3.1/Ges/Res/HotelImage/1.jpg");
        //arrList.Add("http://192.168.1.101/VP3.1/Ges/Res/HotelImage/2.jpg");
        //arrList.Add("http://192.168.1.101/VP3.1/Ges/Res/HotelImage/3.jpg");
        //return arrList;
        return this.getImages("../Res/HotelImage", "getHotelImages");
    }

    /// <summary>
    /// 根据图片路径和方法名返回图片链接
    /// </summary>
    /// <param name="strImagePath"></param>
    /// <param name="strMethodName"></param>
    /// <returns></returns>
    private ArrayList getImages(String strImagePath, String strMethodName)
    {
        ArrayList arrList = new ArrayList();

        String strCurUrl = base.Context.Request.Url.ToString();
        strCurUrl = strCurUrl.Replace("Service/ImageService.asmx/" + strMethodName, "");
        strCurUrl = strCurUrl + strImagePath.Replace("../", "") + "/";

        String strPath = Server.MapPath(strImagePath);

        if (Directory.Exists(strPath))
        {
            string[] files = Directory.GetFiles(strPath);
            for (int i = 0; i < files.Length; i++)
            {
                string strFilename = files[i].Substring(files[i].LastIndexOf(@"\") + 1);
                strFilename = strCurUrl + strFilename;
                arrList.Add(strFilename);
            }
        }

        return arrList;
    }


}

