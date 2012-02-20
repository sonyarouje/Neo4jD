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
            T entity = (T)Activator.CreateInstance(typeof(T));
            if (node.GetProperty("clazz")!=typeof(T).ToString())
                throw new InvalidCastException(string.Format("Retrieved object with ID '{0}' is an instance of '{1}' and unable to cast it to '{2}'", id.ToString(), node.GetProperty("clazz"), typeof(T).ToString()));
            typeof(T).GetProperties().Where(pr => pr.CanRead && MapperHelper.IsAnId(pr) == false).ToList().ForEach(property =>
            {
                property.SetValue(entity, MapperHelper.CastPropertyValue(property, node.GetProperty(property.Name)), null);
            });

            entity = MapperHelper.SetIdentity<T>(entity, id);
            return entity;
        }

        private T Get<T>(Node node)
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
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
            //Node node = this.CreateNode<T>(entity);
            //node.Create();
            //return MapperHelper.SetIdentity<T>(entity, node.Id);
        }

        public object Save(object entity)
        {
            Node node = this.CreateNode(entity);
            node.Create();
            return MapperHelper.SetIdentity(entity, node.Id);
        }

        internal Node CreateNode<T>(T entity) where T:class
        {
            Node node = new Node();
            node.AddProperty("clazz", typeof(T).ToString());
            typeof(T).GetProperties().Where(pr => pr.CanRead && MapperHelper.IsAnId(pr) == false).ToList().ForEach(property =>
            {
                if(MapperHelper.IsPrimitive(property.PropertyType))
                    node.AddProperty(property.Name, property.GetValue(entity, null).ToString());
            });

            return node;
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

        /// <summary>
        /// Create relationship between TEntity and TRelated. Before creating the relationship the function
        /// checks whether instance are saved or not. if not saved then it will save both the instance before
        /// creating the relationship.
        /// </summary>
        /// <typeparam name="TEntity">Parent entity Type</typeparam>
        /// <typeparam name="TRelated">Chile entity Type</typeparam>
        /// <param name="entity">Instance of Parent Entity</param>
        /// <param name="related">Instance of Chile Entity</param>
        public void CreateRelationshipTo<TEntity, TRelated>(TEntity entity, TRelated related)
            where TEntity : class
            where TRelated : class
        {
            Node parent = this.SaveAndReturnAsNode<TEntity>(entity);
            Node child = this.SaveAndReturnAsNode<TRelated>(related);
            parent.CreateRelationshipTo(child, typeof(TRelated).ToString());
        }

        public void CreateRelationshipTo<TEntity, TRelated>(TEntity entity, IList<TRelated> relatedList)
            where TEntity : class
            where TRelated : class
        {
            foreach (TRelated related in relatedList)
                this.CreateRelationshipTo<TEntity, TRelated>(entity, related);
        }

        internal Node SaveAndReturnAsNode<T>(T entity) where T:class
        {
            if (entity == null)
                return null;
            int id = MapperHelper.GetIdentity<T>(entity);
            Node node = null;
            if (id <= 0)
            {
                node=this.CreateNode<T>(entity);
                node.Create();
                MapperHelper.SetIdentity<T>(entity, node.Id);
            }
            else
                node = Node.Get(id);

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

        public IList<TRelated> GetRelatedEntities<TEntity, TRelated>(TEntity entity, Type related) where TEntity:class where TRelated:class
        {
            Node node = Node.Get(MapperHelper.GetIdentity<TEntity>(entity));
            GermlinPipe germlin = new GermlinPipe();
            germlin.G.V.Out(related.ToString());
            IList<Node> relatedNodes = node.Filter(germlin);
            IList<TRelated> relatedEntities = new List<TRelated>();
            foreach (Node nodeToConvert in relatedNodes)
                relatedEntities.Add(this.Get<TRelated>(nodeToConvert));
            return relatedEntities;
        }

        public IList<TRelated> GetRelatedEntities<TRelated>(Expression<Func<TRelated>> property, object entity)
        {
            return this.LoadRelatedEntities<TRelated>(entity, ((MemberExpression)property.Body).Member.Name);
        }

        public IList<TRelated> GetRelatedEntities<TRelated>(Expression<Func<ICollection<TRelated>>> property, object entity)
        {
            return this.LoadRelatedEntities<TRelated>(entity, ((MemberExpression)property.Body).Member.Name);
        }

        private IList<TRelated> LoadRelatedEntities<TRelated>(object entity, string memberName)
        {
            Node node = Node.Get(MapperHelper.GetIdentity(entity));
            GermlinPipe germlin = new GermlinPipe();
            germlin.G.V.Out(memberName);
            IList<Node> relatedNodes = node.Filter(germlin);
            IList<TRelated> relatedEntities = new List<TRelated>();
            foreach (Node nodeToConvert in relatedNodes)
                relatedEntities.Add(this.Get<TRelated>(nodeToConvert));
            return relatedEntities;
        }
    }
}
