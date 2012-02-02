using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD;
namespace Net.Graph.Neo4JD.Persistance
{
    public class RelationShipDB
    {
        private readonly GraphRequest _graphRequest;
        public RelationShipDB()
        {
            _graphRequest = new GraphRequest();
        }
        public Relationship GetRelationship(string relationShipId)
        {
            var uri = UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(), "db/data/relationship/" + relationShipId);
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

        public void Delete(Relationship relationShipToDelete)
        {
            _graphRequest.Post(RequestType.DELETE, relationShipToDelete.GetLocation(), null);
        }
    }
}
