using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Germlin.Pipes
{
    public class FilterPipe:Pipe
    {
        public FilterPipe(string valueToFilter)
        {
            base.SetPipeValue(valueToFilter);
        }
    }
}
