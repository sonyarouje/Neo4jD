using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD;
namespace Net.Graph.Neo4JD.Persistance
{
    public class NodeRepo:Repository
    {
        public Node GetNode(Uri nodeUri)
        {
            Node node = new Node();
            node.SetLocation(nodeUri);
            return this.GetNode(node);
        }

        public Node GetNode(string nodeId)
        {
            var uri = UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(), "db/data/node/" + nodeId);
            Node node = new Node();
            node.SetLocation(uri);
            return this.GetNode(node);
        }

        public Node GetNode(Node node)
        {
            var result = _graphRequest.Post(RequestType.GET, node.GetLocation(), null);
            node.SetResult(result);
            return node;
        }

        public Node CreateNode(Node node)
        {
            var uri = UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(), "db/data/node");
            var result = _graphRequest.Post(RequestType.POST, uri, node.GetProperties());
            node.SetLocation(result.GetLocation());

            return node;
        }

        public RequestResult GetRelatedNodes(Node node, string direction)
        {
            RequestResult result = _graphRequest.Post(RequestType.GET, new Uri(string.Concat(node.GetLocation(), @"/relationships/", direction)), null);
            return result;
        }

        public RequestResult GetRestTraversalResult(Node node, string query)
        {
            Console.WriteLine(query);
            var uri = UriHelper.ConcatUri(node.GetLocation(), "/traverse/node");
            var result = _graphRequest.Post(RequestType.POST, uri, query);
            return result;
        }
    }
}
