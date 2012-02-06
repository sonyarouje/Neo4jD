using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD.Traversal.Rest.Pipes;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest
{
    public class RestAPI:INeo4jQuery
    {
        IList<RestBasePipe> _restPipes;
        private RelationShipFilter _relationShipFilter;
        public RestAPI()
        {
            _restPipes = new List<RestBasePipe>();
        }

        private RestAPI Add(RestBasePipe pipe)
        {
            _restPipes.Add(pipe);
            return this;
        }

        public RestAPI Order(OrderType order)
        {
            return this.Add(new Order(order));
        }

        public RestAPI Filter(PropertyFilter filter)
        {
            return this.Add(new Filter(filter));
        }

        public RestAPI RelationShips(RelationshipDirection relationShipDirection, string relationShipType)
        {
            if (_relationShipFilter == null)
            {
                _relationShipFilter = new RelationShipFilter();
                this.Add(_relationShipFilter);
            }
            _relationShipFilter.Add(new RelationFilter(relationShipDirection,relationShipType));
            return this;
        }

        public RestAPI Uniqueness(UniquenessType uniqueness)
        {
            return this.Add(new Uniqueness(uniqueness));
        }

        public RestAPI MaxDepth(int maxDepth)
        {
            return this.Add(new MaxDepth(maxDepth));
        }

        public override string ToString()
        {
            JObject restQuery = new JObject();
            foreach (RestBasePipe pipe in _restPipes)
            {
                restQuery.Add(pipe.GetJsonObject());
            }

            return restQuery.ToString();
        }
    }
}
