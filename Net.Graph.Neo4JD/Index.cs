using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD.Persistance;
using Net.Graph.Neo4JD.JsonParser;
using Net.Graph.Neo4JD.Exceptions;
using Net.Graph.Neo4JD.Traversal.Index;
namespace Net.Graph.Neo4JD
{
    public class Index
    {
        private IndexRepo _indexRepo = null;
        private Uri _location = null;

        internal Index()
        {
            this._indexRepo = new IndexRepo();
        }

        public static Index Create(string indexName)
        {
            IndexRepo indexRepoTmp = new IndexRepo();
            return indexRepoTmp.Create(indexName);
        }

        //public static Index GetAutoIndex()
        //{
        //    IndexRepo indexRepoTmp = new IndexRepo();
        //    return indexRepoTmp.GetAutoIndex();
        //}

        public static Index Get(string indexName)
        {
            IndexRepo indexRepoTmp = new IndexRepo();
            return indexRepoTmp.GetIndex(indexName);
        }

        internal void SetLocation(Uri location)
        {
            _location = location;
        }

        public Uri GetLocation()
        {
            return _location;
        }

        public Index Add(Node node, string key, string value)
        {
            if (node.GetLocation() == null)
                throw new InvalidNodeException();
            _indexRepo.AddNodeToIndex(this,node, key, value);

            return this;
        }

        public void RemoveNode(Node node)
        {
            if (node.GetLocation() == null)
                throw new InvalidNodeException();

            _indexRepo.RemoveNodeFromIndex(this,node);
        }

        public IList<Node> Search(IndexQuery query)
        {
            RequestResult result= _indexRepo.Search(this, query.ToString());
            IndexSearchResultParser indexSearchResultParser = new IndexSearchResultParser(result);
            return indexSearchResultParser.GetFilteredNodes();
        }
    }
}
