using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD.Traversal.Index.Pipes;
namespace Net.Graph.Neo4JD.Traversal.Index
{
    public class IndexQuery
    {
        IList<IndexBasePipe> _pipes;
        public IndexQuery()
        {
            _pipes = new List<IndexBasePipe>();
        }

        private IndexQuery Add(IndexBasePipe pipe)
        {
            _pipes.Add(pipe);
            return this;
        }

        public IndexQuery GetKey(string propertyName)
        {
            return this.Add(new PropertyPipe(propertyName));
        }

        public IndexQuery StartsWith(string propertyValue)
        {
            return this.Add(new StartsWith(propertyValue));
        }

        public IndexQuery Equals(string propertyValue)
        {
            return this.Add(new Equals(propertyValue));
        }

        public IndexQuery AND()
        {
            return this.Add(new And());
        }

        public IndexQuery OR()
        {
            return this.Add(new Or());
        }

        public override string ToString()
        {
            return this.ExtractQuery();
        }

        private string ExtractQuery()
        {
            StringBuilder query = new StringBuilder();
            foreach (IndexBasePipe pipe in _pipes)
                query.Append(pipe.ToString());

            return string.Format("?query={0}", query.ToString());
        }
    }
}
