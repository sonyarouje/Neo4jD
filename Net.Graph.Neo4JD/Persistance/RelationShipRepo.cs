using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD;
namespace Net.Graph.Neo4JD.Persistance
{
    public class RelationShipRepo:Repository
    {
        public RelationShipRepo()
        {
        }
        public Relationship GetRelationship(string relationShipId)
        {
            var uri = UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(), "db/data/relationship/" + relationShipId);
            return this.GetRelationship(uri);
        }

        public Relationship GetRelationship(Uri uri)
        {
            var result = _graphRequest.Post(RequestType.GET, uri, null);
            Relationship relationship = new Relationship(result);
            return relationship;
        }

        public Relationship CreateRelationship(Node parent, Relationship relationShip)
        {
            var result = _graphRequest.Post(RequestType.POST, new Uri(string.Concat(parent.GetLocation(), @"/relationships")), relationShip.GetProperties());
            relationShip.SetLocation(result.GetLocation());
            return relationShip;
        }

    }
}
