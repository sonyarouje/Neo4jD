using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Net.Graph.Neo4J.Client.Entity;
namespace Net.Graph.Neo4J.Client
{
    public class GraphDB
    {
        private Uri _baseUri;
        public GraphDB(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public Node CreateNode(Node node)
        {
            GraphRequest graphRequest = new GraphRequest();
            var uri = this.ConcatUri(_baseUri, "db/data/node");
            var result = graphRequest.Post(RequestType.POST, uri, node.GetProperties());
            node.SetLocation(result.GetLocation());

            return node;
        }

        public Node GetNode(string nodeId)
        {
            var uri = this.ConcatUri(_baseUri, "db/data/node/" + nodeId);
            Node node = new Node();
            node.SetLocation(uri);
            return this.GetNode(node);
        }

        public Node GetNode(Node node)
        {
            GraphRequest graphRequest = new GraphRequest();
            var result = graphRequest.Post(RequestType.GET, node.GetLocation(), null);
            node.SetResult(result);
            return node;
        }
        //public Relationship CreateRelationship(Relationship relationShip)
        //{
        //    GraphRequest graphRequest = new GraphRequest();
        //    var result = graphRequest.Post(RequestType.POST, relationShip.GetStartNodeLocation(), relationShip.GetProperties());
        //    relationShip.SetLocation(result.GetLocation());
        //    return relationShip;
        //}

        public Relationship GetRelationship(string relationShipId)
        {
            GraphRequest graphRequest = new GraphRequest();
            var uri = this.ConcatUri(_baseUri, "db/data/relationship/" + relationShipId);
            var result = graphRequest.Post(RequestType.GET, uri, null);
            Relationship relationship = new Relationship(result);
            return relationship;
        }

        private Uri ConcatUri(Uri uri, params object[] uriParts)
        {
            var stringBuilder = new StringBuilder();
            foreach (var uriPart in uriParts)
            {
                stringBuilder.Append(uriPart).Append("/");
            }
            var toAdd = stringBuilder.ToString().TrimEnd('/');
            return new Uri(string.Concat(uri.AbsoluteUri, toAdd));
        }
    }
}
