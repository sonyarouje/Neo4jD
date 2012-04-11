using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
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
            node.AddProperty("FirstName", "Kunjachan");
            node.AddProperty("LastName", "Arouje");
            node.Create();

            Assert.IsNotNull(node.GetLocation());
            Console.WriteLine(node.GetLocation().ToString());

            Node mother = new Node();
            mother.AddProperty("FirstName", "Marry").AddProperty("LastName", "Treassa").Create();
            Assert.IsNotNull(mother.GetLocation());
            Console.WriteLine(mother.GetLocation().ToString());
        }

        [TestCase]
        public void GetNodes()
        {
            Node mother = Node.Get(2);
            Assert.AreEqual(2,mother.Id);

            Node father = Node.Get(1);
            Assert.AreEqual(1, father.Id);
        }

        [TestCase]
        public void SetProperty()
        {
            Node mother = Node.Get(2);
            Assert.AreEqual("Marry", mother.GetProperty("FirstName"));
            //mother.SetProperty("FirstName", "Marry");
            //mother.SetProperty("SecondName", "Tressa");

            Node motherWithProf = Node.Get(2);
            Assert.AreEqual("Marry", motherWithProf.GetProperty("FirstName"));
        }

        [TestCase]
        public void UpdateProperty()
        {
            Node mother = Node.Get(2);
            mother.UpdateProperties("Profession", "Ast Head Mistress");

            Node motherWithUpdatedProf = Node.Get(2);
            Assert.AreEqual("Ast Head Mistress", motherWithUpdatedProf.GetProperty("Profession"));
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
            nodeToDelete.AddProperty("FirstName", "Delete").AddProperty("LastName", "Node");
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
            Node node = Node.Get(1);
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
            Node mother = Node.Get(2);
            Console.WriteLine(mother.GetLocation().ToString());

            Node father = Node.Get(1);
            Assert.IsNotNull(father);
            Assert.IsNotNull(father.GetLocation());
            Console.WriteLine(father.GetLocation().ToString());

            Relationship relationship= father.CreateRelationshipTo(mother, "wife");
            Assert.IsNotNull(relationship.GetLocation());
        }

        [TestCase]
        public void CreateSonRelationship()
        {
            Node son = new Node();
            son.AddProperty("FirstName", "Sony");
            son.AddProperty("LastName", "Arouje");
            son.Create();

            Node father = Node.Get(1);
            Assert.IsNotNull(father);
            Assert.IsNotNull(father.GetLocation());
            Console.WriteLine(father.GetLocation().ToString());

            Relationship relationship = father.CreateRelationshipTo(son, "son");
            Assert.IsNotNull(relationship.GetLocation());
        }

        [TestCase]
        public void RelationShipFromThisNode()
        {
            Node father = Node.Get(1);
            IList<Node> relations = father.Out();
            Assert.AreEqual(1, relations.Count);
            foreach(Node node in relations)
                Console.WriteLine(node.Id.ToString());
        }

        [TestCase]
        public void RelationShipToThisNode()
        {
            Node mother = Node.Get(2);
            IList<Node> relations = mother.In();
            Assert.AreEqual(1, relations.Count);
            foreach (Node node in relations)
                Console.WriteLine(node.Id.ToString());
        }

        [TestCase]
        public void GetAllPaths()
        {
            Node father = Node.Get(1);
            Node mother = Node.Get(2);

            IList<Relationship> relationShips = father.GetAllPathsTo(mother);
            Assert.IsNotNull(relationShips);
            Assert.AreEqual(1, relationShips.Count);
            foreach (Relationship relation in relationShips)
                Assert.AreEqual("wife", relation.GetType());
        }
    }
}
