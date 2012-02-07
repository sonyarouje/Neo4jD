using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Traversal.Index.Pipes
{
    public class PropertyPipe:IndexBasePipe
    {
        private readonly string _propertyName;
        public PropertyPipe(string propertyName)
        {
            _propertyName = propertyName;
        }

        public override string ToString()
        {
            return string.Format("{0}:", _propertyName);
        }
    }
}
