using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.Traversal.Germlin;
namespace Net.Graph.Neo4JD.EntityMapper
{
    public class NodeMapper
    {
        public T Get<T>(int id) where T:class
        { 
            Node node = Node.Get(id);
            return Get<T>(node);
        }

        private static T Get<T>(Node node) where T:class
        {
            T entity = LazyLoader.EnableLazyLoading<T>();  //(T)Activator.CreateInstance(typeof(T));
            if (node.GetProperty("clazz") != typeof(T).ToString())
                throw new InvalidCastException (string.Format("Retrieved object with ID '{0}' is an instance of '{1}' and unable to cast it to '{2}'",node.Id.ToString(), node.GetProperty("clazz"), typeof(T).ToString()));
            typeof(T).GetProperties().Where(pr => pr.CanRead && MapperHelper.IsAnId(pr) == false).ToList().ForEach(property =>
            {
                property.SetValue(entity, MapperHelper.CastPropertyValue(property, node.GetProperty(property.Name)), null);
            });

            entity = MapperHelper.SetIdentity<T>(entity, node.Id);
            return entity;
        }

        public void Delete<T>(T entity) where T : class
        {
            Node node = Node.Get(MapperHelper.GetIdentity<T>(entity));
            node.Delete();
        }

        public T Save<T>(T entity) where T:class
        {
            RelationshipCreateHelper relationShipHelper = new RelationshipCreateHelper(this);
            return (T)relationShipHelper.SaveNodeWithRelationShip(entity);
        }

        public static IList<TRelated> LoadRelatedEntitiesWithId<TRelated>(int id, string memberName) where TRelated : class
        {
            Node node = Node.Get(id);
            GermlinPipe germlin = new GermlinPipe();
            germlin.G.V.Out(memberName);
            IList<Node> relatedNodes = node.Filter(germlin);
            IList<TRelated> relatedEntities = new List<TRelated>();
            foreach (Node nodeToConvert in relatedNodes)
                relatedEntities.Add(Get<TRelated>(nodeToConvert));
            return relatedEntities;
        }

        internal Node CreateNode(object entity)
        {
            Node node = new Node();
            node.AddProperty("clazz", entity.GetType().ToString());
            entity.GetType().GetProperties().Where(pr => pr.CanRead && MapperHelper.IsAnId(pr) == false).ToList().ForEach(property =>
            {
                if (MapperHelper.IsPrimitive(property.PropertyType))
                    node.AddProperty(property.Name, property.GetValue(entity, null).ToString());
            });

            return node;
        }

        internal Node SaveAndReturnAsNode(object entity)
        {
            if (entity == null)
                return null;
            int id = MapperHelper.GetIdentity(entity);
            Node node = null;
            if (id <= 0)
            {
                node = this.CreateNode(entity);
                node.Create();
                MapperHelper.SetIdentity(entity, node.Id);
            }
            else
                node = Node.Get(id);

            return node;
        }
    }
}
