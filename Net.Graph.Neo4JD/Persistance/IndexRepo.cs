using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Persistance
{
    public class IndexRepo
    {
        protected readonly GraphRequest _graphRequest;
        public IndexRepo()
        {
            _graphRequest = new GraphRequest();
        }

        internal Index GetAutoIndex()
        {
            Index index = new Index();
            index.SetLocation(UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(),"db/data/index/auto/node"));

            return index;
        }

        internal Index GetIndex(string indexName)
        {
            Index index = new Index();
            index.SetLocation(UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(), "db/data/index/node/" + indexName));
            return index;
        }

        internal Index Create(string indexName)
        {
            JObject indexNameJson = new JObject();
            indexNameJson.Add("name", new JValue(indexName));

            Index index = new Index();
            index.SetLocation(UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(), "db/data/index/node/" + indexName));
            var response = _graphRequest.Post(RequestType.POST, UriHelper.ConcatUri(GraphEnvironment.GetBaseUri(),"db/data/index/node/") , indexNameJson.ToString());
            //Console.WriteLine(response.GetResponseData());
            return index;
        }

        internal void AddNodeToIndex(Index index, Node nodeToAdd, string key, string value)
        {
            JObject indexParams =new JObject();
            indexParams.Add("value", new JValue(value));
            indexParams.Add("uri", new JValue(nodeToAdd.GetLocation().ToString()));
            indexParams.Add("key", new JValue(key));
            var response = _graphRequest.Post(RequestType.POST, index.GetLocation(), indexParams.ToString());
        }

        internal void RemoveNodeFromIndex(Index index, Node nodeToRemove)
        {
            var response = _graphRequest.Post(RequestType.DELETE,  new Uri(index.GetLocation() +"/" + nodeToRemove.Id.ToString()), null);
        }

        internal RequestResult Search(Index indexToSearch, string searchQuery)
        {
            var response = _graphRequest.Post(RequestType.GET, UriHelper.ConcatUri(indexToSearch.GetLocation(),searchQuery), null);
            return response;
        }
    }
}
