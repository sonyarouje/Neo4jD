using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
namespace Net.Graph.Neo4JD.EntityMapper
{
    public static class ProxyCloner
    {
        private static ReflectionHelper _reflectionHelper;

        static ProxyCloner()
        {
            _reflectionHelper = new ReflectionHelper();
        }
        public static object ConvertProxyToEntity(object proxy)
        {
            if (IsAProxyObject(proxy))
            {
                Type type = (proxy as IProxyTargetAccessor).DynProxyGetTarget().GetType().BaseType;
                object entity = _reflectionHelper.GetInstance(type);
                entity = _reflectionHelper.CloneProperties(proxy, entity);
                return entity;
            }
            else
                return proxy;
        }

        private static bool IsAProxyObject(object proxy)
        {
            if (proxy is IProxyTargetAccessor == false)
                return false;
            else if ((proxy as IProxyTargetAccessor).DynProxyGetTarget().GetType() == null)
                return false;
            else
                return true;
        }

    }
}
