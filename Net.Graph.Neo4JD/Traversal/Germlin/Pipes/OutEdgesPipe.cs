using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Traversal.Germlin.Pipes
{
    public class OutEdgesPipe:Pipe
    {
        public OutEdgesPipe(string label):this()
        {
            base.SetPipeValue(label);
        }

        public OutEdgesPipe()
        {
            base.SetPipeName("outE");
        }
    }
}
