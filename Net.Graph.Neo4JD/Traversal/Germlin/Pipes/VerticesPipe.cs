using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Traversal.Germlin.Pipes
{
    public class VerticesPipe:Pipe
    {
        public VerticesPipe(int id)
        {
            base.SetPipeName("V");
            base.SetPipeValue(id);
        }
    }
}
