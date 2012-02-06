using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public enum RelationshipDirection
    {
        in_direction=0,
        out_direction=1,
        all_direction=2
    }
    public class RelationFilter:RestBasePipe
    {
        private readonly RelationshipDirection _direction;
        private readonly string _relationShipTypeFilter;
        public RelationFilter(RelationshipDirection relationShipDirection, string relationShipTypeFilter)
        {
            _direction=relationShipDirection;
            _relationShipTypeFilter = relationShipTypeFilter;
        }

        public override object GetJsonObject()
        {
            JObject relationShip = new JObject();
            string direction = _direction.ToString().Split("_".ToCharArray())[0];

            relationShip.Add("direction", new JValue(direction));
            relationShip.Add("type", new JValue(_relationShipTypeFilter));

            return relationShip;
        }
    }
}
