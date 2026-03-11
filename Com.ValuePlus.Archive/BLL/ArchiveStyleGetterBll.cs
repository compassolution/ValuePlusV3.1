using System;
using System.Collections;
using System.Text;
using System.IO;
using Com.ValuePlus.Utils.Cryptography;
using Com.ValuePlus.Archive.Config;
using Com.ValuePlus.Archive.Entity;

namespace Com.ValuePlus.Archive.BLL
{
    public class ArchiveStyleGetterBll
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取特定模板的配置
        /// </summary>
        /// <param name="strTID"></param>
        /// <returns></returns>
        public static Entity_ArchiveStyle GetTemplateStylesEntity(String strTID)
        {
            Hashtable hs = Com.ValuePlus.SysParams.ArchiveStyleParamsGetter.GetArchiveStyleParams(strTID);
            Entity_ArchiveStyle entity = new Entity_ArchiveStyle();

            if ((hs!=null)&&(hs.Count>0))
            {
                entity.CellNum = hs["COLNUM"].ToString();
                entity.TableWidth = hs["TABLEWIDTH"].ToString();
                entity.LabelWidth = hs["LABELWIDTH"].ToString();
                entity.PageStyle = hs["PAGETYPE"].ToString();
                entity.IsCloseAfterSaved = hs["ISCLOSE"].ToString();
                entity.IsRealTimeAlert = hs["ISREALTIMEALERT"] == null ? "0" : hs["ISREALTIMEALERT"].ToString();
                entity.DefaultFilterPID = hs["DefaultFilterPID"].ToString();
                entity.IsSearchByPY = hs["IsSearchByPY"].ToString();
            }
            else
            {
                entity = GetTemplateDefaultStylesEntity();
            }
            return entity;
        }

        /// <summary>
        /// 获取模板默认配置
        /// </summary>
        /// <param name="strTID"></param>
        /// <returns></returns>
        public static Entity_ArchiveStyle GetTemplateDefaultStylesEntity()
        {
            Hashtable hs = Com.ValuePlus.SysParams.ArchiveStyleParamsGetter.GetArchiveStyleParams("DEFAULT");
            Entity_ArchiveStyle entity = new Entity_ArchiveStyle();

            if ((hs != null) && (hs.Count > 0))
            {
                entity.CellNum = hs["COLNUM"].ToString();
                entity.TableWidth = hs["TABLEWIDTH"].ToString();
                entity.LabelWidth = hs["LABELWIDTH"].ToString();
                entity.PageStyle = hs["PAGETYPE"].ToString();
                entity.IsCloseAfterSaved = hs["ISCLOSE"].ToString();
                entity.IsRealTimeAlert = hs["ISREALTIMEALERT"]==null?"0":hs["ISREALTIMEALERT"].ToString();
                entity.DefaultFilterPID = hs["DefaultFilterPID"].ToString();
                entity.IsSearchByPY = hs["IsSearchByPY"].ToString();
            }
            else
            {
                entity.CellNum = "6";
                entity.TableWidth = "100";
                entity.LabelWidth = "15%";
                entity.PageStyle = "0";
                entity.IsCloseAfterSaved = "1";
                entity.IsRealTimeAlert = "0";
                entity.DefaultFilterPID = "";
                entity.IsSearchByPY = "1";
            }
            return entity;
        }

    }
}
