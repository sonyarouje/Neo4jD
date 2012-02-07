using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Traversal.Index.Pipes
{
    public class Equals:IndexBasePipe
    {
        private readonly string _propertyValue;
        public Equals(string propertyValue)
        {
            _propertyValue = propertyValue;
        }

        public override string ToString()
        {
            return _propertyValue;
        }
    }
}
