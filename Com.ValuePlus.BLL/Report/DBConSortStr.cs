using System;
using System.Collections.Generic;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.BLL.Report
{
    public class DBConSortStr
    {
        //数据库服务器
        private String _server = "";
        //数据库名称
        private String _database = "";
        //数据库访问用户
        private String _user = "";
        //数据库访问密码
        private String _password = "";

        public DBConSortStr()
        {
            String strDbConnect = BaseConfig.Instance.GetConnectionString();
            String[] strArray = strDbConnect.Split(new char[] { ';' });
            for (int i = 0; i < strArray.Length; i++)
            {
                String[] strArray2 = strArray[i].Split(new char[] { '=' });
                String str2 = strArray2[0];
                if (str2 != null)
                {
                    str2 = str2.ToLower();
                    if (!(str2 == "data source"))
                    {
                        if (str2 == "initial catalog")
                        {
                            goto Label_00CA;
                        }
                        if (str2 == "user id")
                        {
                            goto Label_00D5;
                        }
                        if (str2 == "pwd")
                        {
                            goto Label_00E0;
                        }
                    }
                    else
                    {
                        this._server = strArray2[1];
                    }
                }
                continue;
            Label_00CA:
                this._database = strArray2[1];
                continue;
            Label_00D5:
                this._user = strArray2[1];
                continue;
            Label_00E0:
                this._password = strArray2[1];
            }
        }

        public String Database
        {
            get
            {
                return this._database;
            }
        }

        public String Password
        {
            get
            {
                return this._password;
            }
        }

        public String Server
        {
            get
            {
                return this._server;
            }
        }

        public String User
        {
            get
            {
                return this._user;
            }
        }
    }
}
