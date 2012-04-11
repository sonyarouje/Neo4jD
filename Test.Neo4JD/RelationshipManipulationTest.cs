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
    public class RelationshipManipulationTest
    {
        [SetUp]
        public void Initialize()
        {
            GraphEnvironment.SetBaseUri("http://localhost:7474/");
        }

        [TestCase]
        public void GetRelationShipById()
        {
            Relationship relation = Relationship.Get(5);
            Assert.AreEqual(5, relation.Id);
        }

        [TestCase]
        public void GetRelationShipAndTestVertices()
        {
            Relationship relation = Relationship.Get(5);
            Node startNode = relation.StartNode();
            Node endNode = relation.EndNode();

            Assert.AreEqual(19, startNode.Id);
            Assert.AreEqual(20, endNode.Id);
        }

        [TestCase]
        public void UpdateRelationship()
        {
            Relationship relationShip = Relationship.Get(5);
            relationShip.SetProperty("Name", "New prop");

            Relationship updatedRelationShip = Relationship.Get(5);
            Assert.AreEqual("New prop", updatedRelationShip.GetProperty("Name"));
        }
        [TestCase]
        public void TryDeleteAnInvalidRelationship()
        {
            
        }
    }
}
