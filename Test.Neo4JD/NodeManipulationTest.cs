using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Core;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.Exceptions;
namespace Test.Neo4jClient
{
    [TestFixture]
    public class NodeManipulationTest
    {
        [SetUp]
        public void Initialize()
        {
            GraphEnvironment.SetBaseUri("http://localhost:7474/");
        }

        [TestCase]
        public void CreateNode()
        {
            Node node = new Node();
            node.SetProperty("FirstName", "Kunjachan");
            node.SetProperty("LastName", "Arouje");
            node.Create();

            Assert.IsNotNull(node.GetLocation());
            Console.WriteLine(node.GetLocation().ToString());

            Node mother = new Node();
            mother.SetProperty("FirstName", "Marry").SetProperty("LastName", "Treassa").Create();
            mother.SetProperty("FirstName", "Viji").SetProperty("LastName", "P").Create();
            mother.Create();
            Assert.IsNotNull(mother.GetLocation());
            Console.WriteLine(mother.GetLocation().ToString());
        }

        [TestCase]
        public void GetNodes()
        {
            Node mother = Node.Get(20);
            Assert.AreEqual(20,mother.Id);

            Node father = Node.Get(19);
            Assert.AreEqual(19, father.Id);
        }

        [TestCase]
        [ExpectedException(typeof(NodeNotFoundException))]
        public void GetUnknownNodeExceptionTest()
        {
            Node unknownNode = Node.Get(300);
        }

        [TestCase]
        public void DeleteNode()
        {
            Node nodeToDelete = new Node();
            nodeToDelete.SetProperty("FirstName", "Delete").SetProperty("LastName", "Node");
            nodeToDelete.Create();
            int id = nodeToDelete.Id;
            Console.WriteLine(nodeToDelete.GetLocation().ToString());

            nodeToDelete.Delete();

            Node accessDeletedNode = Node.Get(id);
            Assert.AreEqual(0, accessDeletedNode.Id);
        }

        [TestCase]
        [ExpectedException (typeof(NodeDeleteException))]
        public void DeleteNodeThatHasRelationship()
        {
            Node node = Node.Get(19);
            node.Delete();
        }

        [TestCase]
        [ExpectedException (typeof(InvalidNodeException))]
        public void TryDelete_A_Node_That_Is_NotFetched()
        {
            Node node=new Node();
            node.Delete();
        }

        [TestCase]
        public void CreateRelationship()
        {
            Node mother = Node.Get(20);
            Console.WriteLine(mother.GetLocation().ToString());

            Node father = Node.Get(19);
            Assert.IsNotNull(father);
            Assert.IsNotNull(father.GetLocation());
            Console.WriteLine(father.GetLocation().ToString());

            Relationship relationship= father.CreateRelationshipTo(mother, "wife");
            Assert.IsNotNull(relationship.GetLocation());
        }

        [TestCase]
        public void RelationShipFromThisNode()
        {
            Node father = Node.Get(19);
            IList<Node> relations = father.Out();
            Assert.AreEqual(2, relations.Count);
            foreach(Node node in relations)
                Console.WriteLine(node.Id.ToString());
        }

        [TestCase]
        public void RelationShipToThisNode()
        {
            Node mother = Node.Get(20);
            IList<Node> relations = mother.In();
            Assert.AreEqual(2, relations.Count);
            foreach (Node node in relations)
                Console.WriteLine(node.Id.ToString());
        }
    }
}
