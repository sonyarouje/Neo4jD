using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Net.Graph.Neo4JD.Persistance;
namespace Net.Graph.Neo4JD.JsonParser
{
    public class IndexSearchResultParser
    {
        private RequestResult _result;
        public IndexSearchResultParser(RequestResult result)
        {
            _result = result;
        }

        internal IList<Node> GetFilteredNodes()
        {
            IList<Node> selectedNodes = new List<Node>();
            JArray array = JArray.Parse(_result.GetResponseData());
            foreach (var item in array)
            {
                Node node = Node.Get(item["self"].ToString());
                selectedNodes.Add(node);
            }
            return selectedNodes;
        }
    }
}
