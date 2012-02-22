using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
namespace Net.Graph.Neo4JD.EntityMapper
{
    public class EntityConfiguration<T>:BaseConfiguration
    {
        public EntityConfiguration():base(typeof(T))
        {
            
        }
        public EntityConfiguration<T> RelatedTo<R>(Expression<Func<T,R>>  expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            base.AddExpression(typeof(R), body.Member.Name, false);
            return this;
        }
        public EntityConfiguration<T> RelatedTo<R>(Expression<Func<T, ICollection<R>>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            base.AddExpression(typeof(R), body.Member.Name, true);
            return this;
        }

        public EntityConfiguration<T> Leaf()
        {
            base.AddExpression(null, string.Empty, false);
            return this;
        }
    }
}
