using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD.Germlin.Pipes;
namespace Net.Graph.Neo4JD.Germlin
{
    public class GermlinPipe
    {
        IList<Pipes.Pipe> _pipes = null;

        public GermlinPipe()
        {
            _pipes = new List<Pipes.Pipe>();
        }

        private GermlinPipe Add(Pipe pipe)
        {
            _pipes.Add(pipe);
            return this;
        }

        public GermlinPipe G()
        {
            return this.Add(new GraphPipe());
        }

        public GermlinPipe V(int id)
        {
            return this.Add(new VerticesPipe(id));
        }

        public GermlinPipe Out(string label)
        {
            return this.Add(new OutPipe(label));
        }

        public GermlinPipe In(string label)
        {
            return this.Add(new InPipe(label));
        }

        public GermlinPipe Filter(string property, FilterPipe filter)
        {
            return this.Add(new PropertyPipe(property, filter));
        }
        public override string ToString()
        {
            return PipeParser.Parse(_pipes);
        }

    }
}
