using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.EntityMapper
{
    public class EntityRelation
    {
        public EntityRelation(Type entity)
        {
            this.DirectSubEntities = new List<SubEntityDetails>();
            this.Entity = entity;
        }
        internal Type Entity { get; private set; }

        internal IList<SubEntityDetails> DirectSubEntities { get; private set; }
        internal void AddSubEntity(Type child, string memberName, bool isCollection)
        {
            SubEntityDetails entityDetails = new SubEntityDetails(child, memberName, isCollection);
            this.DirectSubEntities.Add(entityDetails);
        }
    }

    public class SubEntityDetails
    {
        public SubEntityDetails(Type child, string memberName, bool isCollection)
        {
            this.Child = child;
            this.MemberName = memberName;
            this.IsCollection = isCollection;
        }

        public Type Child { get; private set; }
        public string MemberName { get; private set; }
        public bool IsCollection { get; private set; }
    }
}
