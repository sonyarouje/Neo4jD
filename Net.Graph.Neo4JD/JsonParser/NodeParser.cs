using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.Persistance;
namespace Net.Graph.Neo4JD.JsonParser
{
    public class NodeParser:ParserBase
    {
        public NodeParser(Dictionary<string,object> keyValuePair):base(keyValuePair)
        {

        }

        public override void JsonToEntity(RequestResult result, BaseEntity entity)
        {
            base.JsonToEntity(result, entity);
        }
            
    }
}
