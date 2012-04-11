using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Net.Graph.Neo4JD.Persistance;
using Net.Graph.Neo4JD.JsonParser;
namespace Net.Graph.Neo4JD
{
    public class Relationship:BaseEntity
    {
        private string _toNodeLocation;
        private string _type;
        private string _startNodePath = string.Empty;
        private string _endNodePath = string.Empty;
        private RelationShipRepo _relationshipRepo;
        private NodeRepo _nodeRepo;
        private Relationship()
        {
            _relationshipRepo = new RelationShipRepo();
            _nodeRepo = new NodeRepo();
            base.SetRepository(_relationshipRepo);
        }
        
        internal Relationship(Node startNode, Node endNode, string relationShipType):this()
        {
            _toNodeLocation = endNode.GetLocation().ToString();
            _type = relationShipType;
        }

        internal Relationship(RequestResult result):this()
        {
            JsonParser.ParserBase nodeParser = new JsonParser.RelationshipParser(this._keyValuePair);
            nodeParser.JsonToEntity(result, this);
        }

        public static Relationship Get(int relationshipID)
        {
            Persistance.RelationShipRepo db = new RelationShipRepo();
            return db.GetRelationship(relationshipID.ToString());
        }

        internal override void SetLocation(Uri location)
        {
            if (location.ToString().Contains("node"))
                throw new InvalidCastException(string.Format("Unable to cast Node to Relationship. The type is a Node, Location: {0}", location.ToString()));

            base.SetLocation(location);
        }

        public void SetType(string type)
        {
            this._type = type;
        }

        public string GetType()
        {
            return this._type;
        }

        public override void Delete()
        {
            if (this.GetLocation() == null)
                throw new NullReferenceException("Location is null. The relationship might not be valid, get a valid relation from the db.");
            _relationshipRepo.Delete(this);
        }

        internal void SetVertices(string startNodePath, string endNodePath)
        {
            _startNodePath = startNodePath;
            _endNodePath = endNodePath;
        }

        public Node StartNode()
        {
            return _nodeRepo.GetNode(new Uri(_startNodePath));
        }

        public Node EndNode()
        {
            return _nodeRepo.GetNode(new Uri(_endNodePath));
        }

        public override string GetProperties()
        {
            string data= base.GetProperties().ToString();
            JObject props = new JObject();
            props.Add("to", new JValue(_toNodeLocation));
            props.Add("type", new JValue(_type));
            //props.Add("data", new JValue(data));

            return props.ToString();
        }
    }
}
