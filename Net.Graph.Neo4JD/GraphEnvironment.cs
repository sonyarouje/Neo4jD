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
            if (_baseUri == null)
                throw new NullReferenceException("The base URI is not set. Set base Uri using GraphEnvironment.SetBaseUri()");
            return _baseUri;
        }
    }
}
