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
            //BinaryExpression body = (BinaryExpression)expr.Body;
            //ParameterExpression left = (ParameterExpression)body.Left;
            //ParameterExpression right = (ParameterExpression)body.Right;
            //Console.WriteLine(expression.Body.ToString());
            //Console.WriteLine(expression.Body.Type.ToString());
            //Console.WriteLine(typeof(T).ToString());
            //Console.WriteLine(body.Member.Name);
            base.AddExpression(typeof(R), body.Member.Name, false);
            return this;
        }
        public EntityConfiguration<T> RelatedTo<R>(Expression<Func<T, ICollection<R>>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            //Console.WriteLine(expression.Body.ToString());
            //Console.WriteLine(expression.Body.Type.ToString());
            //Console.WriteLine(typeof(T).ToString());
            //Console.WriteLine(body.Member.Name);
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
