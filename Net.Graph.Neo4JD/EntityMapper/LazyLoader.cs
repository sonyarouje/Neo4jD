using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
namespace Net.Graph.Neo4JD.EntityMapper
{
    public static class LazyLoader
    {
        private static readonly ProxyGenerator _generator=new ProxyGenerator();
        public static T EnableLazyLoading<T>() where T:class
        {
            LazyLoadInterceptor interceptor = new LazyLoadInterceptor();
            var proxy = _generator.CreateClassProxy<T>(interceptor);
            return proxy;
        }
    }
}
