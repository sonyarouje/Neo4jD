using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Net.Graph.Neo4JD.EntityMapper
{
    internal class MapperHelper
    {
        internal static T SetIdentity<T>(T entity, int id)
        {
            return (T)SetIdentityToEntity(entity, id);
        }

        internal static object SetIdentity(object entity, int id)
        {
            return SetIdentityToEntity(entity, id);
        }

        private static object SetIdentityToEntity(object entity, int id)
        {
            entity.GetType().GetProperties().Where(pr => pr.CanRead && IsAnId(pr) == true).ToList().ForEach(property =>
            {
                property.SetValue(entity, id, null);
            });

            return entity;
        }
        internal static int GetIdentity<T>(T entity) where T : class
        {
            return GetIdentityFromEntity(entity);
        }

        internal static int GetIdentity(object entity)
        {
            return GetIdentityFromEntity(entity);
        }

        private static int GetIdentityFromEntity(object entity)
        {
            if (entity == null)
                return 0;

            object propertyValue = 0;
            entity.GetType().GetProperties().Where(pr => pr.CanRead && IsAnId(pr) == true).ToList().ForEach(property =>
            {
                propertyValue = property.GetValue(entity, null);
            });

            return Convert.ToInt32(propertyValue);
        }

        internal static bool IsAnId(PropertyInfo propInfo)
        {
            EntityId idProp = (EntityId)propInfo.GetCustomAttributes(false).FirstOrDefault(attr => attr is EntityId);
            if (idProp == null)
                return false;
            return true;
        }

        internal static object CastPropertyValue(PropertyInfo property, string value)
        {
            if (property == null || String.IsNullOrEmpty(value))
                return null;
            if (property.PropertyType.IsEnum)
            {
                Type enumType = property.PropertyType;
                if (Enum.IsDefined(enumType, value))
                    return Enum.Parse(enumType, value);
            }
            if (property.PropertyType == typeof(bool))
                return value == "1" || value == "true" || value == "on" || value == "checked";
            else if (property.PropertyType == typeof(Uri))
                return new Uri(Convert.ToString(value));
            else
                return Convert.ChangeType(value, property.PropertyType);
        }

        internal static bool IsPrimitive(Type t)
        {
            return new[] 
            { 
                typeof(string), 
                typeof(char),
                typeof(byte),
                typeof(ushort),
                typeof(short),
                typeof(uint),
                typeof(int),
                typeof(ulong),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(DateTime),
            }.Contains(t);
        }
    }
}
