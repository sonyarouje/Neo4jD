using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD.Traversal.Germlin.Pipes;
namespace Net.Graph.Neo4JD.Traversal.Germlin
{
    public class GermlinPipe:INeo4jQuery
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

        public GermlinPipe G
        {
            get { return this.Add(new GraphPipe()); }
        }

        public GermlinPipe V
        {
            get { return this.Add(new VerticesPipe()); }
        }

        public GermlinPipe Out()
        {
            return this.Add(new OutPipe());
        }

        public GermlinPipe Out(string label)
        {
            return this.Add(new OutPipe(label));
        }

        public GermlinPipe OutV()
        {
            return this.Add(new OutVPipe());
        }

        public GermlinPipe OutE()
        {
            return this.Add(new OutEdgesPipe());
        }
        public GermlinPipe OutE(string label)
        {
            return this.Add(new OutEdgesPipe(label));
        }

        public GermlinPipe In(string label)
        {
            return this.Add(new InPipe(label));
        }
        public GermlinPipe InV()
        {
            return this.Add(new InVPipe());
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
