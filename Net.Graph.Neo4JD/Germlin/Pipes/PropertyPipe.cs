using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Germlin.Pipes
{
    public class PropertyPipe:Pipe 
    {
        string _property;
        Pipe _filter;
        public PropertyPipe(string property, Pipe filter)
        {
            _property = property;
            _filter = filter;
        }

        public override string ToString()
        {
            return string.Format("filter{{it.{0}=='{1}'}}", _property, _filter.GetPipeValue());
        }
    }
}
