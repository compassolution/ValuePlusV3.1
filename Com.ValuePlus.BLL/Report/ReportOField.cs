
using System;
using System.Runtime.Serialization;

namespace Com.ValuePlus.BLL.Report
{
    [Serializable]
    public class ReportOField
    {
        public string alias;
        public string DEFAULTVALUE;
        public string GID;
        public string PID;
        public string prefix;
        public string SID;
        public string TID;
        public string TXT;

        public ReportOField()
        {
        }

        protected ReportOField(SerializationInfo info, StreamingContext context)
        {
            this.alias = info.GetString("alias");
            this.TID = info.GetString("TID");
            this.GID = info.GetString("GID");
            this.SID = info.GetString("SID");
            this.PID = info.GetString("PID");
            this.TXT = info.GetString("TXT");
            this.DEFAULTVALUE = info.GetString("DEFAULTVALUE");
            this.prefix = info.GetString("prefix");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            ReportOField field = (ReportOField)obj;
            return (this.alias == field.alias);
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("alias", this.alias);
            info.AddValue("TID", this.TID);
            info.AddValue("GID", this.GID);
            info.AddValue("SID", this.SID);
            info.AddValue("PID", this.PID);
            info.AddValue("TXT", this.TXT);
            info.AddValue("DEFAULTVALUE", this.DEFAULTVALUE);
            info.AddValue("prefix", this.prefix);
        }
    }
}
