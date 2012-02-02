using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Net.Graph.Neo4JD.Persistance;
namespace Net.Graph.Neo4JD
{
    public class Node:BaseEntity
    {
        Persistance.NodeDB _db;
        Persistance.RelationShipDB _relationShipDb;
        public Node()
        {
            _db = new Persistance.NodeDB();
            _relationShipDb = new Persistance.RelationShipDB();
        }

        public Node(RequestResult result):this()
        {
            this.SetResult(result);
        }

        public static Node Get(int id)
        {
            try
            {
                Persistance.NodeDB db = new Persistance.NodeDB();
                return db.GetNode(id.ToString());
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.NodeNotFoundException(ex,id);
            }
        }

        public override BaseEntity Create()
        {
            return _db.CreateNode(this);
        }

        public override void Delete()
        {
            if (this.GetLocation() == null || this.Id <= 0)
                throw new Exceptions.InvalidNodeException();

            _db.DeleteNode(this);
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
            return _relationShipDb.CreateRelationship(this, relationShip);
        }

        public IList<Node> In()
        {
            return this.GetRelationshipNodes("in","start");
        }

        public IList<Node> Out()
        {
            return this.GetRelationshipNodes("out","end");
        }

        private IList<Node> GetRelationshipNodes(string direction, string element)
        {
            if (this.GetLocation() == null || this.Id <= 0)
                throw new Exceptions.InvalidNodeException();

            RequestResult result = _db.GetRelatedNodes(this, direction);

            IList<Node> childs = new List<Node>();
            JArray array = JArray.Parse(result.GetResponseData());
            foreach (var tkn in array)
            {
                Node node = new Node();
                node.SetLocation(new Uri(tkn[element].ToString()));
                node = _db.GetNode(node);
                childs.Add(node);
            }
            return childs;
        }
    }
}
