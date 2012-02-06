using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public enum UniquenessType
    {
        node_global=0,
        none=1,
        relationship_global=2,
        node_path=3,
        relationship_path=4
    }
    public class Uniqueness:RestBasePipe
    {
        private UniquenessType _uniquenessUserSelected;
        public Uniqueness(UniquenessType uniqueness)
        {
            _uniquenessUserSelected = uniqueness;
        }

        public override object GetJsonObject()
        {
            return new JProperty("uniqueness", _uniquenessUserSelected.ToString());
        }
    }
}
