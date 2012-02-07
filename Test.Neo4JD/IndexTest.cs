using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core;
using NUnit.Framework;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.Traversal.Index;
namespace Test.Neo4jClient
{
    [TestFixture]
    public class IndexTest
    {
        [SetUp]
        public void Initialize()
        {
            GraphEnvironment.SetBaseUri("http://localhost:7474/");
        }

        [TestCase]
        public void Create_Index()
        {
            Index test = Index.Create("TestIndex");
            Index fav = Index.Create("favaourite");
        }

        [TestCase]
        public void Create_A_FavouriteIndex_And_AddNode()
        {
            Index fav = Index.Get("favaourites");
            Node node = Node.Get(1);
            fav.Add(node, "FirstName", "dad");

            Node node1 = Node.Get(2);
            fav.Add(node1, "FirstName", "mom");
        }

        [TestCase]
        public void Remove_Node_FromIndex()
        {
            Index fav = Index.Get("favaourites");
            Node node = Node.Get(1);
            fav.RemoveNode(node);
        }

        [TestCase]
        public void Search_Index()
        {
            Index fav = Index.Get("favaourites");
            IndexQuery qry = new IndexQuery();
            qry.GetKey("FirstName").StartsWith("mo").OR().GetKey("FirstName").StartsWith("dad");
            IList<Node> nodes= fav.Search(qry);
            Assert.AreEqual(2, nodes.Count);
        }
    }
}
