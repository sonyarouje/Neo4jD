using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public class RestAPI
    {
        IList<RestBasePipe> _restPipes;
        public RestAPI()
        {
            _restPipes = new List<RestBasePipe>();
        }

        private RestAPI Add(RestBasePipe pipe)
        {
            _restPipes.Add(pipe);
            return this;
        }

        public RestAPI Order(OrderPipe order)
        {
            return this.Add(order);
        }

        public RestAPI Filter(FilterPipe filter)
        {
            return this.Add(filter);
        }

        public RestAPI RelationShips(RelationShipPipe relationShips)
        {
            return this.Add(relationShips);
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
