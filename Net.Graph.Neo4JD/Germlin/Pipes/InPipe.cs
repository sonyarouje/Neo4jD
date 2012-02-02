using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Germlin.Pipes
{
    public class InPipe:Pipe
    {
        public InPipe(string value)
        {
            base.SetPipeName("in");
            base.SetPipeValue(value);
        }
    }
}
