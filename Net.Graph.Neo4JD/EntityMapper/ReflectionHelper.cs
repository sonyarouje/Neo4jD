using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Net.Graph.Neo4JD.EntityMapper
{
    public class ReflectionHelper
    {
        public bool CanCast(string fromClass, string toClass)
        {
            if (fromClass == toClass)
                return true;

            Type frmType = ReturnInstance(fromClass);
            Type toType = ReturnInstance(toClass);
            if (frmType == null || toType == null)
                return false;
            else if (frmType.IsAssignableFrom(toType))
                return true;
            else if (toType.IsAssignableFrom(frmType))
                return true;
            else
                return false;
        }

        public Type ReturnInstance(string className)
        {
            Type typ = null;
            if (ModelBuilder.GetAssemblies().Count <= 0)
                throw new Exception("No assemblies added to ModelBuilder. Call ModelBuilder.AddAssembly() function and add all the assemblies contains classes that needs to be persisted to Neo4j");

            foreach (Assembly assm in ModelBuilder.GetAssemblies())
            {
                typ = assm.GetType(className);
                if(typ!=null)
                    break;
            }

            return typ;
        }

        public object InvokePrivateStaticGenericMethod(object obj, string methodName, Type genericType, object[] args)
        {
            return obj.GetType().GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(genericType).Invoke(null, args);
        }
    }
}
