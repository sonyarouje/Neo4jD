using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.Core.Internal;

namespace Net.Graph.Neo4JD.EntityMapper
{
    public  class LazyLoadInterceptor:IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            bool isIntercepted = false;
            string propName = invocation.Method.Name.Replace("get_", "");

            if (invocation.Method.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase))
            {
                Type type = invocation.TargetType.GetProperty(propName).PropertyType;
                if (MapperHelper.IsPrimitive(type) == false)
                {
                    isIntercepted = true;
                    invocation.Proceed();
                    if (invocation.ReturnValue == null)
                        invocation.ReturnValue = this.DoLazyLoad(type, invocation.Proxy, propName);

                    Console.WriteLine(type.ToString());
                }
            }
            if (isIntercepted == false)
                invocation.Proceed();
        }

        private object DoLazyLoad(Type type, object entity, string propertyName)
        {
            var castedEntity = entity as IProxyTargetAccessor;
            Type genericType = type.IsGenericType ? type.GetGenericArguments()[0] : type;
            MethodInfo method = typeof(NodeMapper).GetMethod("LoadRelatedEntitiesWithId");
            MethodInfo generic = method.MakeGenericMethod(genericType);
            int id = MapperHelper.GetIdentity(castedEntity.DynProxyGetTarget());

            object result = generic.Invoke(null, new object[] { id, propertyName });

            if (type.Namespace != "System.Collections.Generic")
                result= ((IEnumerable)result).Cast<object>().ToArray().FirstOrDefault();

            return result;
        }

    }
}
