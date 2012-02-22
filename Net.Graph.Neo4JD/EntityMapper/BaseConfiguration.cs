using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.EntityMapper
{
    public class BaseConfiguration
    {
        private EntityRelation _relation;

        public BaseConfiguration(Type parent)
        {
            _relation = new EntityRelation(parent);
        }

        protected void AddExpression(Type child, string member, bool isCollection)
        {
            if(child!=null)
                _relation.AddSubEntity(child, member, isCollection);
        }

        public EntityRelation GetRelatedEntityConfiguration()
        {
            return _relation;
        }

        public Type EntityType { get { return _relation.EntityType; } }
    }
}
