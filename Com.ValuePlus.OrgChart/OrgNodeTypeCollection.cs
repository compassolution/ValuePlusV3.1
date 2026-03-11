
using System;
using System.Collections;
using System.Reflection;

namespace Com.ValuePlus.OrgChart
{
    public class OrgNodeTypeCollection : CollectionBase
    {
        private object ObjectOrgNodeTypeCollection;

        public OrgNodeTypeCollection()
        {
            this.ObjectOrgNodeTypeCollection = null;
        }

        public OrgNodeTypeCollection(object parent)
        {
            this.ObjectOrgNodeTypeCollection = parent;
        }

        public void Add(OrgNodeType item)
        {
            base.List.Add(item);
        }

        public void AddAt(int index, OrgNodeType item)
        {
            base.List.Insert(index, item);
        }

        public bool Contains(OrgNodeType item)
        {
            return base.List.Contains(item);
        }

        public int IndexOf(OrgNodeType item)
        {
            return base.List.IndexOf(item);
        }

        private void InitOrgNodeTypeCollection(OrgNodeType type1)
        {
            type1.ObjectOrgNodeType = this.Parent;
        }

        protected override void OnInsert(int index, object value)
        {
            this.InitOrgNodeTypeCollection((OrgNodeType)value);
            base.OnInsert(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            this.InitOrgNodeTypeCollection((OrgNodeType)newValue);
            base.OnSet(index, oldValue, newValue);
        }

        public void Remove(OrgNodeType item)
        {
            base.List.Remove(item);
        }

        public OrgNodeType this[int index]
        {
            get
            {
                return (OrgNodeType)base.List[index];
            }
        }

        public object Parent
        {
            get
            {
                return this.ObjectOrgNodeTypeCollection;
            }
            set
            {
                this.ObjectOrgNodeTypeCollection = value;
                if (value != null)
                {
                    foreach (OrgNodeType type in base.List)
                    {
                        this.InitOrgNodeTypeCollection(type);
                    }
                }
            }
        }
    }
}
