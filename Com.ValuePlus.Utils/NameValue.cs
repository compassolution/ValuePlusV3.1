using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Utils
{
    public class NameValue
    {
        private String _name;
        private Object _value;

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public Object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
