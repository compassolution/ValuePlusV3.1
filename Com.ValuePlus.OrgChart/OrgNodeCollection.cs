using System;
using System.Collections;
using System.Reflection;

namespace Com.ValuePlus.OrgChart
{
    public class OrgNodeCollection : CollectionBase
    {
        private object ObjectOrgNodeCollection;

        public OrgNodeCollection()
        {
        }

        public OrgNodeCollection(object parent)
        {
            this.ObjectOrgNodeCollection = parent;
        }

        public void Add(OrgNode item)
        {
            base.List.Add(item);
        }

        public void AddAt(int index, OrgNode item)
        {
            base.List.Insert(index, item);
        }

        public bool Contains(OrgNode item)
        {
            return base.List.Contains(item);
        }

        public int IndexOf(OrgNode item)
        {
            return base.List.IndexOf(item);
        }

        private void InitOrgNodeCollection(OrgNode node1)
        {
            node1.Parent = this.Parent;
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            this.InitOrgNodeCollection((OrgNode)newValue);
            base.OnSet(index, oldValue, newValue);
        }

        public void Remove(OrgNode item)
        {
            base.List.Remove(item);
        }

        public OrgNode this[int index]
        {
            get
            {
                return (OrgNode)base.List[index];
            }
        }

        public object Parent
        {
            get
            {
                return this.ObjectOrgNodeCollection;
            }
            set
            {
                this.ObjectOrgNodeCollection = value;
            }
        }
    }
}
