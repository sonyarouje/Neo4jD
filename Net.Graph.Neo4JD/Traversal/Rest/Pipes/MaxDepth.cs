using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public class MaxDepth:RestBasePipe
    {
        private int _maxDepth = 0;
        
        public MaxDepth(int maxDepth)
        {
            _maxDepth = maxDepth;
        }

        public override object GetJsonObject()
        {
            return new JProperty("max_depth", _maxDepth);
        }
    }
}
