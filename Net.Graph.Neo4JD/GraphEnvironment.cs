using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD
{
    public class GraphEnvironment
    {
        private static Uri _baseUri = null;
        public static void SetBaseUri(string baseUri)
        {
            _baseUri = new Uri(baseUri);
        }

        public static Uri GetBaseUri()
        {
            return _baseUri;
        }
    }
}
