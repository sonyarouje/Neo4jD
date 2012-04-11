using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Net.Graph.Neo4JD;
namespace Net.Graph.Neo4JD.Persistance
{
    public class NodeRepo:Repository
    {
        private RelationShipRepo _relationShipRepo;

        public NodeRepo()
        {
            _relationShipRepo = new RelationShipRepo();
        }

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

        public RequestResult GetRestExecutionResult(Node node, string query)
        {
            //Console.WriteLine(query);
            var uri = UriHelper.ConcatUri(node.GetLocation(), "/traverse/node");
            var result = _graphRequest.Post(RequestType.POST, uri, query);
            return result;
        }

        public RequestResult GetGermlinExecutionResult(Node node, string query)
        {
            query = query.Replace("#id", node.Id.ToString());
            var uri = UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(), "db/data/ext/GremlinPlugin/graphdb/execute_script");
            var result = _graphRequest.Post(RequestType.POST, uri, query);
            return result;
        }

        public IList<Node> CreateNodeArray(string element, RequestResult result)
        {
            IList<Node> childs = new List<Node>();
            JArray array = JArray.Parse(result.GetResponseData());
            foreach (var tkn in array)
            {
                Node node = new Node();
                node.SetLocation(new Uri(tkn[element].ToString()));
                node = this.GetNode(node);
                childs.Add(node);
            }
            return childs;
        }

        public IList<Relationship> GetAllPathsTo(Node toNode, RequestResult result)
        {
            IList<Relationship> relationShips = new List<Relationship>();
            JArray array = JArray.Parse(result.GetResponseData());
            foreach (var tkn in array)
            {
                Node node = new Node();
                node.SetLocation(new Uri(tkn["end"].ToString()));
                if (node.Id == toNode.Id)
                {
                    Relationship relationShip = _relationShipRepo.GetRelationship(new Uri(tkn["self"].ToString()));
                    relationShips.Add(relationShip);
                }
            }

            return relationShips;
        }
    }
}
