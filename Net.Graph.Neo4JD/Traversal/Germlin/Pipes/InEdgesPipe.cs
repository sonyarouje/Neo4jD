using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Traversal.Germlin.Pipes
{
    public class InEdgesPipe:Pipe
    {
        public InEdgesPipe(string label)
        {
            base.SetPipeName("inE");
            base.SetPipeValue(label);
        }
    }
}
