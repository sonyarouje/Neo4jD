using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Traversal.Index.Pipes
{
    public class StartsWith:IndexBasePipe
    {
        private readonly string _startsWith;
        public StartsWith(string propertyValue)
        {
            _startsWith = propertyValue;
        }

        public override string ToString()
        {
            return string.Format("{0}*", _startsWith);
        }
    }
}
