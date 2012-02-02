using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD;
namespace Net.Graph.Neo4JD.Persistance
{
    public class NodeDB
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
            GraphRequest graphRequest = new GraphRequest();
            var result = graphRequest.Post(RequestType.GET, node.GetLocation(), null);
            node.SetResult(result);
            return node;
        }

        public Node CreateNode(Node node)
        {
            GraphRequest graphRequest = new GraphRequest();
            var uri = UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(), "db/data/node");
            var result = graphRequest.Post(RequestType.POST, uri, node.GetProperties());
            node.SetLocation(result.GetLocation());

            return node;
        }

        public void DeleteNode(Node node)
        {
            try
            {
                GraphRequest graphRequest = new GraphRequest();
                var result = graphRequest.Post(RequestType.DELETE, node.GetLocation(), null);
            }
            catch (Exception ex)
            {
                throw new Exceptions.NodeDeleteException(node);
            }
        }

        public RequestResult GetRelatedNodes(Node node, string direction)
        {
            GraphRequest request = new GraphRequest();
            RequestResult result = request.Post(RequestType.GET, new Uri(string.Concat(node.GetLocation(), @"/relationships/", direction)), null);
            return result;
        }

    }
}
