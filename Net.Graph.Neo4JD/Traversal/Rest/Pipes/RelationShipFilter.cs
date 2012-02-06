using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public class RelationShipFilter:RestBasePipe
    {
        private IList<RestBasePipe> _relationShips;

        public RelationShipFilter()
        {
            _relationShips = new List<RestBasePipe>();
        }

        public RelationShipFilter(List<RelationFilter> relationFilters):this()
        {
            _relationShips = (IList<RestBasePipe>)relationFilters;
        }

        public RelationShipFilter Add(RelationFilter relationFilter)
        {
            _relationShips.Add(relationFilter);
            return this;
        }

        public override object GetJsonObject()
        {
            JArray relationships = new JArray();
            foreach (RestBasePipe relationFilter in _relationShips)
            {
                relationships.Add(relationFilter.GetJsonObject());
            }

            return new JProperty("relationships", relationships);
        }
    }
}
