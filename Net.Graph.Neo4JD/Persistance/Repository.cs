using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Persistance
{
    public class Repository
    {
        protected readonly GraphRequest _graphRequest;

        public Repository()
        {
            _graphRequest = new GraphRequest();
        }

        public virtual void Delete(BaseEntity entity)
        {
            try
            {
                _graphRequest.Post(RequestType.DELETE, entity.GetLocation(), null);
            }
            catch (Exception ex)
            {
                if (entity.GetType() == typeof(Node))
                    throw new Exceptions.NodeDeleteException((Node)entity);
                else
                    throw;
            }
        }

        public virtual BaseEntity SetProperty(BaseEntity entity, string propertyName)
        {
            var uri = UriHelper.ConcatUri(entity.GetLocation(), "/properties", propertyName);
            var result = _graphRequest.Post(RequestType.PUT, uri, string.Format(@"""{0}""",entity.GetProperty(propertyName)) );
            return entity;
        }

        public virtual BaseEntity UpdateProperty(BaseEntity entity, string propertyToUpdate)
        {
            var uri = UriHelper.ConcatUri(entity.GetLocation(), "/properties");
            var result = _graphRequest.Post(RequestType.PUT, uri, propertyToUpdate);

            return entity;
        }
    }
}
