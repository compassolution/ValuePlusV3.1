using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Archive.Entity;
using System.Data;

namespace Com.ValuePlus.Archive.BLL
{
    public class SetDataToEntityBll
    {
        /// <summary>
        /// 数据行DataRow填充到TB_HRTMPSG实体变量中返回实体对象
        /// </summary>
        /// <param name=dr></param>
        /// <returns>Entity_TB_HRTMPSG</returns>
        public static Entity_TB_HRTMPSG SetDataToEntity_TB_HRTMPSG(DataRow dr)
        {
            Entity_TB_HRTMPSG entity = new Entity_TB_HRTMPSG();
            if (dr != null)
            {
                entity.TID = dr["TID"].ToString();
                entity.SID = dr["SID"].ToString();
                entity.GID = dr["GID"].ToString();
                entity.GDESC = dr["GDESC"].ToString();
                entity.GDESCCHS = dr["GDESCCHS"].ToString();
                entity.GTYPE = dr["GTYPE"] == DBNull.Value ? 0 : int.Parse(dr["GTYPE"].ToString());
                entity.GORDER = dr["GORDER"] == DBNull.Value ? 0 : int.Parse(dr["GORDER"].ToString()); ;
                entity.GLIMIT = dr["GLIMIT"] == DBNull.Value ? 0 : int.Parse(dr["GLIMIT"].ToString());
                entity.GCOUNT = dr["GCOUNT"] == DBNull.Value ? 0 : int.Parse(dr["GCOUNT"].ToString());
                entity.GWIDTH = dr["GWIDTH"] == DBNull.Value ? 0 : int.Parse(dr["GWIDTH"].ToString());
                entity.GRIGHT = dr["GRIGHT"] == DBNull.Value ? 0 : int.Parse(dr["GRIGHT"].ToString());
                entity.GSLCT = dr["GSLCT"].ToString();
                entity.GPAGE = dr["GPAGE"] == DBNull.Value ? 0 : int.Parse(dr["GPAGE"].ToString());
                entity.GRCOUNT = dr["GRCOUNT"] == DBNull.Value ? 0 : int.Parse(dr["GRCOUNT"].ToString());
                entity.DEFAULTCOLUMN = dr["DEFAULTCOLUMN"] == DBNull.Value ? 0 : int.Parse(dr["DEFAULTCOLUMN"].ToString());
                entity.USERPAGE = dr["USERPAGE"].ToString();
                entity.ISLARGE = dr["ISLARGE"] == DBNull.Value ? 0 : int.Parse(dr["ISLARGE"].ToString());
            }
            return entity;
        }

        /// <summary>
        /// 数据行DataRow填充到TB_HRTMPSD实体变量中返回实体对象
        /// </summary>
        /// <param name=dr></param>
        /// <returns>Entity_TB_HRTMPSD</returns>
        public static Entity_TB_HRTMPSD SetDataToEntity_TB_HRTMPSD(DataRow dr)
        {
            Entity_TB_HRTMPSD entity = new Entity_TB_HRTMPSD();
            if (dr != null)
            {
                entity.TID = dr["TID"].ToString();
                entity.SID = dr["SID"].ToString();
                entity.GID = dr["GID"].ToString();
                entity.PID = dr["PID"].ToString();
                entity.PDESC = dr["PDESC"].ToString();
                entity.PDESCCHS = dr["PDESCCHS"].ToString();
                entity.PTYPE = dr["PTYPE"].ToString();
                entity.PLEN = dr["PLEN"] == DBNull.Value ? 0 : int.Parse(dr["PLEN"].ToString());
                entity.PPREC = dr["PPREC"] == DBNull.Value?0: int.Parse(dr["PPREC"].ToString());
                entity.PNULL = dr["PNULL"] == DBNull.Value ? 0 : int.Parse(dr["PNULL"].ToString());
                entity.PDEFAULT = dr["PDEFAULT"].ToString();
                entity.PISKEY = dr["PISKEY"] == DBNull.Value ? 0 : int.Parse(dr["PISKEY"].ToString());
                entity.PCTRL = dr["PCTRL"] == DBNull.Value ? 0 : int.Parse(dr["PCTRL"].ToString());
                entity.PCTRLID = dr["PCTRLID"].ToString();
                entity.PCTRLD = dr["PCTRLD"].ToString();
                entity.PORDER = dr["PORDER"] == DBNull.Value ? 0 : int.Parse(dr["PORDER"].ToString());
                entity.PRIGHT = dr["PRIGHT"] == DBNull.Value ? 0 : int.Parse(dr["PRIGHT"].ToString());
                entity.PSYS = dr["PSYS"] == DBNull.Value ? 0 : int.Parse(dr["PSYS"].ToString());
                entity.PLIST = dr["PLIST"] == DBNull.Value ? 0 : int.Parse(dr["PLIST"].ToString());
                entity.PWIDTH = dr["PWIDTH"] == DBNull.Value ? 0 : int.Parse(dr["PWIDTH"].ToString());
                entity.PFONTL = dr["PFONTL"].ToString();
                entity.PFONTC = dr["PFONTC"].ToString();
                entity.PAGGR = dr["PAGGR"].ToString();
                entity.PAGDEST = dr["PAGDEST"].ToString();
                entity.PMAST = dr["PMAST"].ToString();
                entity.PSAVE = dr["PSAVE"] == DBNull.Value ? 0 : int.Parse(dr["PSAVE"].ToString());
            }
            return entity;
        }


    }
}
