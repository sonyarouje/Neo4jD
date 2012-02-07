using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Net.Graph.Neo4JD.Persistance;
using Net.Graph.Neo4JD.Traversal;
namespace Net.Graph.Neo4JD
{
    public class Node:BaseEntity
    {
        Persistance.NodeRepo _nodeRepo;
        Persistance.RelationShipRepo _relationShipRepo;
        public Node()
        {
            _nodeRepo = new Persistance.NodeRepo();
            _relationShipRepo = new Persistance.RelationShipRepo();
            base.SetRepository(_nodeRepo);
        }

        public Node(RequestResult result):this()
        {
            this.SetResult(result);
        }

        public static Node Get(int id)
        {
            try
            {
                Persistance.NodeRepo db = new Persistance.NodeRepo();
                return db.GetNode(id.ToString());
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.NodeNotFoundException(ex,id);
            }
        }

        public static Node Get(string location)
        {
            Persistance.NodeRepo db = new Persistance.NodeRepo();
            return db.GetNode(new Uri(location));
        }

        public override BaseEntity SetProperty(string propertyName, string propertyValue)
        {
            base.AddProperty(propertyName, propertyValue);
            return _nodeRepo.SetProperty(this, propertyName);
        }

        public override BaseEntity Create()
        {
            return _nodeRepo.CreateNode(this);
        }

        public override void Delete()
        {
            if (this.GetLocation() == null || this.Id <= 0)
                throw new Exceptions.InvalidNodeException();

            _nodeRepo.Delete(this);
        }
        internal void SetResult(RequestResult result)
        {
            JsonParser.NodeParser nodeParser = new JsonParser.NodeParser(this._keyValuePair);
            nodeParser.JsonToEntity(result, this);
        }

        public Relationship CreateRelationshipTo(Node node, string relationShipType)
        {
            if ((this.GetLocation()==null) || (node.GetLocation()==null))
                throw new Exception("Parent or child node is not created. Create both node before creating the relation");
            
            Relationship relationShip = new Relationship(this, node, relationShipType);
            return _relationShipRepo.CreateRelationship(this, relationShip);
        }

        public IList<Node> In()
        {
            return this.GetRelationshipNodes("in","start");
        }

        public IList<Node> Out()
        {
            return this.GetRelationshipNodes("out","end");
        }

        public IList<Node> Filter(INeo4jQuery query)
        {
            RequestResult result= _nodeRepo.GetRestTraversalResult(this, query.ToString());
            return CreateNodeArray("self",result);
        }

        private IList<Node> GetRelationshipNodes(string direction, string element)
        {
            if (this.GetLocation() == null || this.Id <= 0)
                throw new Exceptions.InvalidNodeException();

            RequestResult result = _nodeRepo.GetRelatedNodes(this, direction);

            return CreateNodeArray(element, result);
        }

        private IList<Node> CreateNodeArray(string element, RequestResult result)
        {
            IList<Node> childs = new List<Node>();
            JArray array = JArray.Parse(result.GetResponseData());
            foreach (var tkn in array)
            {
                Node node = new Node();
                node.SetLocation(new Uri(tkn[element].ToString()));
                node = _nodeRepo.GetNode(node);
                childs.Add(node);
            }
            return childs;
        }
    }
}
