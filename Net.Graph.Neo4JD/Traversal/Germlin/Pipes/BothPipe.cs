using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Traversal.Germlin.Pipes
{
    public class BothPipe:Pipe
    {
        public BothPipe(string label)
        {
            base.SetPipeName("both");
            base.SetPipeValue(label);
        }
    }
}
