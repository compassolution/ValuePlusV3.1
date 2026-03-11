using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Com.ValuePlus.Common
{
    /// <summary>
    /// 可排序的HashTable对象
    /// 什么顺序加进去就按什么顺序输出
    /// 取值时需要用这种方法取值才能按顺序输出 foreach (String strColName in hsTableCol.Keys)
    /// </summary>
    [Serializable]
    public class SortHashTable : Hashtable
    {
        private ArrayList list = new ArrayList();
        public override void Add(object key, object value)
        {
            base.Add(key, value);
            list.Add(key);
        }
        public override void Clear()
        {
            base.Clear();
            list.Clear();
        }
        public override void Remove(object key)
        {
            base.Remove(key);
            list.Remove(key);
        }
        public override ICollection Keys
        {
            get
            {
                return list;
            }
        }
    }
}
