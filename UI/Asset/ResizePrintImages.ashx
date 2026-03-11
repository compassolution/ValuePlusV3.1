<%@ WebHandler Language="C#" Class="ResizePrintImages" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data.Common;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Database;

public class ResizePrintImages : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param

        //String strImageFilePathAndName = "D://Work//Assets//Project//HNLH//App//UserFile//MainImages//惠普多功能激光一体打印机(HP 1536DN).JPG";
        //String strImageFilePathAndName_Save = "D://Work//Assets//Project//HNLH//App//UserFile//MainImages//惠普多功能激光一体打印机(HP 1536DN)1.JPG";
        String strImageFilePath = "D://Work//ValuePlus//Projects//Assets//Projects//ZHYC//App//UserFile//MainImages//";
        String strImageSaveFilePath = "D://Work//ValuePlus//Projects//Assets//Projects//ZHYC//App//UserFile//MainImages//Print//";
        if (strParam.Equals("resize"))
        {
            //调整图片大小到30k左右
            //this.GetThumbnail(context,strImageFilePath,strImageSaveFilePath,30000);
            //this.ConvertJpgToPng(context,strImageFilePath,strImageSaveFilePath);

            this.GetThumbnail_New(context, strImageFilePath, strImageSaveFilePath);
        }
        else if (strParam.Equals("insertdata"))
        {
            this.InsertImagesData(context,strImageSaveFilePath);
        }
    }

    /// <summary>
    /// JPG图片转PNG图片
    /// </summary>
    /// <param name="strImageFilePathAndName">图片地址</param>
    /// <param name="strToSaveImageFilePath">缩略图地址</param>
    /// <param name="p"></param>
    public void ConvertJpgToPng(HttpContext context,string strImageFilePath, string strToSaveImageFilePath)
    {
        int iAllCount = 0;
        int iSuccessCount = 0;
        if (Directory.Exists(strImageFilePath))
        {
            if (Directory.Exists(strToSaveImageFilePath))
            {
                Directory.Delete(strToSaveImageFilePath,true);
            }
            Directory.CreateDirectory(strToSaveImageFilePath);

            String strEncodePath =  HttpUtility.UrlEncode(strImageFilePath) ;
            DirectoryInfo thisOne = new DirectoryInfo(strImageFilePath);
            FileInfo[] fileInfo = thisOne.GetFiles();
            iAllCount = fileInfo.Length;
            // 遍历所有的文件和目录
            foreach (FileInfo file in fileInfo)
            {
                String strFileName = file.Name;
                String strFileExtName = file.Extension;
                String strFileLength = file.Length.ToString();

                String strImageFilePathAndName = strImageFilePath + strFileName;
                String strToSaveImageFilePathAndName = strToSaveImageFilePath  + strFileName;

                System.Drawing.Image serverImage = System.Drawing.Image.FromFile(strImageFilePathAndName);

                ////新建一个bmp图片
                //using (var bmp = new Bitmap(serverImage.Width, serverImage.Height))
                //{
                //bmp.SetResolution(serverImage.HorizontalResolution, serverImage.VerticalResolution);


                try
                {
                    strToSaveImageFilePathAndName = strToSaveImageFilePathAndName.Replace(strFileExtName, ".png");
                    //bmp.Save(strToSaveImageFilePathAndName, System.Drawing.Imaging.ImageFormat.Png);

                    CompressImage(strImageFilePathAndName,strToSaveImageFilePathAndName,80,1000,false);

                    iSuccessCount = iSuccessCount + 1;
                }
                catch (System.Exception e)
                {
                    //context.Response.Write("图片总数："+iAllCount.ToString()+"；调整成功数量："+iSuccessCount.ToString());
                    log.Error(e.ToString());
                }
                finally
                {
                    serverImage.Dispose();
                    //bmp.Dispose();
                }
                //}
                ////测试用，只处理100个图片
                //if (iSuccessCount >= 10)
                //{
                //    break;
                //}
            }
        }
        context.Response.Write("图片总数："+iAllCount.ToString()+"；调整成功数量："+iSuccessCount.ToString());
    }

    /// <summary>
    /// 无损压缩图片
    /// </summary>
    /// <param name="sFile">原图片地址</param>
    /// <param name="dFile">压缩后保存图片地址</param>
    /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
    /// <param name="size">压缩后图片的最大大小</param>
    /// <param name="sfsc">是否是第一次调用</param>
    /// <returns></returns>
    public static bool CompressImage(string sFile, string dFile, int flag, int size , bool sfsc )
    {
        //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
        FileInfo firstFileInfo = new FileInfo(sFile);
        if (sfsc == true && firstFileInfo.Length < size * 1024)
        {
            firstFileInfo.CopyTo(dFile);
            return true;
        }
        Image iSource = Image.FromFile(sFile);
        ImageFormat tFormat = iSource.RawFormat;
        int dHeight = iSource.Height / 2;
        int dWidth = iSource.Width / 2;
        int sW = 0, sH = 0;
        //按比例缩放
        Size tem_size = new Size(iSource.Width, iSource.Height);
        if (tem_size.Width > dHeight || tem_size.Width > dWidth)
        {
            if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
            {
                sW = dWidth;
                sH = (dWidth * tem_size.Height) / tem_size.Width;
            }
            else
            {
                sH = dHeight;
                sW = (tem_size.Width * dHeight) / tem_size.Height;
            }
        }
        else
        {
            sW = tem_size.Width;
            sH = tem_size.Height;
        }

        Bitmap ob = new Bitmap(dWidth, dHeight);
        Graphics g = Graphics.FromImage(ob);

        g.Clear(Color.WhiteSmoke);
        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

        g.Dispose();

        //以下代码为保存图片时，设置压缩质量
        EncoderParameters ep = new EncoderParameters();
        long[] qy = new long[1];
        qy[0] = flag;//设置压缩的比例1-100
        EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
        ep.Param[0] = eParam;

        try
        {
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICIinfo = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("Png"))
                {
                    jpegICIinfo = arrayICI[x];
                    break;
                }
            }
            if (jpegICIinfo != null)
            {
                ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                FileInfo fi = new FileInfo(dFile);
                if (fi.Length > 1024 * size)
                {
                    flag = flag - 10;
                    CompressImage(sFile, dFile, flag, size, false);
                }
            }
            else
            {
                ob.Save(dFile, tFormat);
            }
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            iSource.Dispose();
            ob.Dispose();
        }
    }

    //把文件转成二进制流出入数据库
    private void InsertImagesData(HttpContext context,string strImageFilePath)
    {
        int iAllCount = 0;
        int iSuccessCount = 0;
        if (Directory.Exists(strImageFilePath))
        {
            String strEncodePath = HttpUtility.UrlEncode(strImageFilePath);
            DirectoryInfo thisOne = new DirectoryInfo(strImageFilePath);
            FileInfo[] fileInfo = thisOne.GetFiles();
            iAllCount = fileInfo.Length;
            // 遍历所有的文件和目录
            foreach (FileInfo file in fileInfo)
            {
                String strFileName = file.Name;
                String strFileLength = file.Length.ToString();
                //String strEncodeFileName = HttpUtility.UrlEncode(strFileName);
                String strImageFilePathAndName = strImageFilePath + strFileName;
                FileStream fs = new FileStream(strImageFilePathAndName, FileMode.Open);
                try
                {
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] byData = br.ReadBytes((int)fs.Length);

                    strFileName = strFileName.Replace(".png", ".jpg");
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append("DELETE FROM TB_AMIMAGE WHERE SACODE IN (select SACODE from AMASSETS_1 WHERE ISNULL(AIMAGE,'') = '" + strFileName + "'); \r\n");
                    sbSql.Append("INSERT INTO TB_AMIMAGE (SACODE,SIMAGE,Base64String) ");
                    sbSql.Append("SELECT SACODE,@ImageData,[dbo].[Fun_GetBase64String](@ImageData) from AMASSETS_1 WHERE ISNULL(AIMAGE,'') = '" + strFileName + "'");

                    //sbSql.Append(";");

                    //sbSql.Append("DELETE FROM [TB_AMIMAGE_MatchName] WHERE ltrim(rtrim(ImageName)) = '"+strFileName+"'; ");
                    //sbSql.Append("INSERT INTO [TB_AMIMAGE_MatchName] (ImageName,SIMAGE) ");
                    //sbSql.Append("SELECT top 1 ltrim(rtrim(AIMAGE)),@ImageData from AMASSETS_1 WHERE ltrim(rtrim(ISNULL(AIMAGE,''))) = '"+strFileName+"'");

                    String strSql = sbSql.ToString();

                    using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
                    {
                        //Com.ValuePlus.Utils.SqlBasicMetaData sqlMetaData = new Com.ValuePlus.Utils.SqlBasicMetaData();
                        //sqlMetaData.CommandSql = strSql;
                        //sqlMetaData.CommandType = CommandType.Text.ToString();

                        //DbParameter[] param = dao.MakeParameter(sqlMetaData);
                        //param[0].Value = byData;
                        //object obj = dao.ExecuteNonQuery(sqlMetaData, param);
                        //if (obj != null)
                        //{
                        //    int btReturn = Convert.ToInt32(obj); ;
                        //}

                        dao.AddParameter("@ImageData", byData, TypeDao.Binary, byData.Length);
                        DbParameter[] param = dao.GetParameters();
                        int iReturn = dao.ExecuteNonQuery(CommandType.Text, strSql, param);

                    }

                    iSuccessCount = iSuccessCount+1;
                }
                catch (System.Exception e)
                {
                    //context.Response.Write("操作异常中止，图片总数："+iAllCount.ToString()+"；写入数据库成功数量："+iSuccessCount.ToString());
                    log.Error(e.ToString());
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }
        context.Response.Write("图片总数："+iAllCount.ToString()+"；写入数据库成功数量："+iSuccessCount.ToString());
    }


    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="strImageFilePathAndName">图片地址</param>
    /// <param name="strToSaveImageFilePath">缩略图地址</param>
    /// <param name="iSizeKB">图片宽度</param>
    /// <param name="p"></param>
    public void GetThumbnail(HttpContext context,string strImageFilePath, string strToSaveImageFilePath,int iSizeKB)
    {
        int iAllCount = 0;
        int iSuccessCount = 0;
        if (Directory.Exists(strImageFilePath))
        {
            if (Directory.Exists(strToSaveImageFilePath))
            {
                Directory.Delete(strToSaveImageFilePath,true);
            }
            Directory.CreateDirectory(strToSaveImageFilePath);

            String strEncodePath =  HttpUtility.UrlEncode(strImageFilePath) ;
            DirectoryInfo thisOne = new DirectoryInfo(strImageFilePath);
            FileInfo[] fileInfo = thisOne.GetFiles();
            iAllCount = fileInfo.Length;
            // 遍历所有的文件和目录
            foreach (FileInfo file in fileInfo)
            {
                String strFileName = file.Name;
                String strFileLength = file.Length.ToString();
                //String strEncodeFileName = HttpUtility.UrlEncode(strFileName);

                String strImageFilePathAndName = strImageFilePath + strFileName;
                String strToSaveImageFilePathAndName = strToSaveImageFilePath  + strFileName;

                System.Drawing.Image serverImage = System.Drawing.Image.FromFile(strImageFilePathAndName);
                double nResizePercent = 0.50;
                //if (int.Parse(strFileLength) > iSizeKB)
                //{
                //    nResizePercent = Convert.ToDouble(Convert.ToDouble(iSizeKB)/Convert.ToDouble((int.Parse(strFileLength))));
                //}

                int width = Convert.ToInt32(serverImage.Width * nResizePercent);
                int height = Convert.ToInt32(serverImage.Height * nResizePercent);
                //画板大小
                int towidth = width;
                int toheight = height;
                //缩略图矩形框的像素点
                int x = 0;
                int y = 0;
                int ow = serverImage.Width;
                int oh = serverImage.Height;

                if (ow > oh)
                {
                    toheight = serverImage.Height * width / serverImage.Width;
                }
                else
                {
                    towidth = serverImage.Width * height / serverImage.Height;
                }

                //新建一个bmp图片
                System.Drawing.Image bm = new System.Drawing.Bitmap(width, height);
                //新建一个画板
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //清空画布并以透明背景色填充
                g.Clear(System.Drawing.Color.White);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(serverImage, new System.Drawing.Rectangle((width - towidth) / 2, (height - toheight) / 2, towidth, toheight),
                    0, 0, ow, oh,
                    System.Drawing.GraphicsUnit.Pixel);
                try
                {
                    //serverImage.Dispose();
                    //File.Delete(strImageFilePathAndName);
                    //以jpg格式保存缩略图
                    bm.Save(strToSaveImageFilePathAndName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    iSuccessCount = iSuccessCount+1;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    serverImage.Dispose();
                    bm.Dispose();
                    g.Dispose();
                }
            }
        }
        context.Response.Write("图片总数："+iAllCount.ToString()+"；调整成功数量："+iSuccessCount.ToString());
    }


    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="strImageFilePath">图片地址</param>
    /// <param name="strToSaveImageFilePath">缩略图地址</param>
    /// <param name="iSizeKB">图片宽度</param>
    /// <param name="p"></param>
    public void GetThumbnail_New(HttpContext context,string strImageFilePath, string strToSaveImageFilePath)
    {
        int iAllCount = 0;
        int iSuccessCount = 0;
        if (Directory.Exists(strImageFilePath))
        {
            if (Directory.Exists(strToSaveImageFilePath))
            {
                Directory.Delete(strToSaveImageFilePath,true);
            }
            Directory.CreateDirectory(strToSaveImageFilePath);

            String strEncodePath =  HttpUtility.UrlEncode(strImageFilePath) ;
            DirectoryInfo thisOne = new DirectoryInfo(strImageFilePath);
            FileInfo[] fileInfo = thisOne.GetFiles();
            iAllCount = fileInfo.Length;
            // 遍历所有的文件和目录
            foreach (FileInfo file in fileInfo)
            {
                String strFileName = file.Name;
                String strFileLength = file.Length.ToString();
                //String strEncodeFileName = HttpUtility.UrlEncode(strFileName);

                String strImageFilePathAndName = strImageFilePath + strFileName;
                String strToSaveImageFilePathAndName = strToSaveImageFilePath  + strFileName;

                System.Drawing.Image serverImage = System.Drawing.Image.FromFile(strImageFilePathAndName);

                try
                {
                    Compress(serverImage, strToSaveImageFilePathAndName, 100, 500);
                    iSuccessCount = iSuccessCount+1;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    serverImage.Dispose();
                }
            }
        }
        context.Response.Write("图片总数："+iAllCount.ToString()+"；调整成功数量："+iSuccessCount.ToString());
    }

    /// <summary>
    /// 图片设置大小并压缩
    /// </summary>
    /// <param name="iSource">源图片</param>
    /// <param name="dFile">压缩后保存位置</param>
    /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>
    /// <param name="maxWidth">最大宽度（如果图片的宽度大于这个值就等比例缩放）</param>
    /// <returns></returns>
    public static bool Compress(Image iSource, string dFile,  int flag,int maxWidth)
    {
        ImageFormat tFormat = iSource.RawFormat;
        int newW = 0, newH = 0;
        if (iSource.Width > maxWidth)
        {
            newW = maxWidth;
            newH = maxWidth * iSource.Height / iSource.Width;
        }
        else {
            newW = iSource.Width;
            newH = iSource.Height;
        }
        //按比例缩放
        using (Bitmap ob = new Bitmap(newW, newH)) {
            using (Graphics g = Graphics.FromImage(ob)) {
                g.Clear(Color.WhiteSmoke);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(iSource, new Rectangle(0, 0, newW, newH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
                g.Dispose();
                //以下代码为保存图片时，设置压缩质量  
                EncoderParameters ep = new EncoderParameters();
                long[] qy = new long[1];
                qy[0] = flag;//设置压缩的比例1-100  
                EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
                ep.Param[0] = eParam;
                try
                {
                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo jpegICIinfo = null;
                    for (int x = 0; x < arrayICI.Length; x++)
                    {
                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {
                            jpegICIinfo = arrayICI[x];
                            break;
                        }
                    }
                    if (jpegICIinfo != null)
                    {
                        ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径  
                    }
                    else
                    {
                        ob.Save(dFile, tFormat);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    iSource.Dispose();
                    ob.Dispose();
                }
            }
        }
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}