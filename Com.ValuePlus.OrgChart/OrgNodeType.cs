using System;
using System.ComponentModel;
using System.Web.UI;

namespace Com.ValuePlus.OrgChart
{
    public class OrgNodeType
    {
        private string NavigateUrlAddress;
        internal object ObjectOrgNodeType;
        private string TypeStyle;

        public override string ToString()
        {
            if (this.Type != string.Empty)
            {
                return this.Type;
            }
            return base.ToString();
        }

        [DefaultValue(""), Category("Behavior"), PersistenceMode(PersistenceMode.Attribute)]
        public string NavigateUrl
        {
            get
            {
                if (this.NavigateUrlAddress != null)
                {
                    return this.NavigateUrlAddress;
                }
                return string.Empty;
            }
            set
            {
                this.NavigateUrlAddress = value;
            }
        }

        public object Parent
        {
            get
            {
                return this.ObjectOrgNodeType;
            }
        }

        [Category("Data"), DefaultValue(""), PersistenceMode(PersistenceMode.Attribute)]
        public string Type
        {
            get
            {
                if (this.TypeStyle != null)
                {
                    return this.TypeStyle;
                }
                return string.Empty;
            }
            set
            {
                this.TypeStyle = value;
            }
        }
    }

}
