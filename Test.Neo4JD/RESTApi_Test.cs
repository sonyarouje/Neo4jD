using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Core;
using Newtonsoft.Json.Linq;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.Traversal.Rest;
using Net.Graph.Neo4JD.Traversal.Rest.Pipes;
namespace Test.Neo4jClient
{
    [TestFixture]
    public class RESTApi_Test
    {
        [SetUp]
        public void Initialize()
        {
            GraphEnvironment.SetBaseUri("http://localhost:7474/");
        }

        private string GetTestRestQuery()
        {
            JObject query = new JObject();
            JProperty orderProp = new JProperty("order", "breadth_first");
            query.Add(orderProp);

            JObject returnFilter = new JObject();
            returnFilter.Add("body", new JValue("position.endNode().getProperty('FirstName').toLowerCase().contains('sony')"));
            returnFilter.Add("language", new JValue("javascript"));
            JProperty filter = new JProperty("return_filter", returnFilter);
            query.Add(filter);

            JArray relationships = new JArray();
            JObject relationShip1 = new JObject();
            relationShip1.Add("direction", new JValue("out"));
            relationShip1.Add("type", new JValue("wife"));
            relationships.Add(relationShip1);

            JObject relationShip2 = new JObject();
            relationShip2.Add("direction", new JValue("all"));
            relationShip2.Add("type", new JValue("loves"));
            relationships.Add(relationShip2);
            JProperty relationShipProp = new JProperty("relationships", relationships);

            query.Add(relationShipProp);

            JProperty uniqueness = new JProperty("uniqueness", "node_global");
            query.Add(uniqueness);
            JProperty maxDepth = new JProperty("max_depth", 2);
            query.Add(maxDepth);
            return query.ToString();
        }
        [TestCase]
        public void RestAPIFormatTest()
        {
            RestAPI r = new RestAPI();
            string qry = r.Order(OrderType.breadth_first)
                .Filter
                (
                    new PropertyFilter().SetPropertyName("FirstName").Contains("sony")
                )
                .RelationShips
                (
                    new RelationShip()
                        .Add(new RelationFilter(RelationshipDirection.out_direction, "wife"))
                        .Add(new RelationFilter(RelationshipDirection.all_direction, "loves"))
                )
                .Uniqueness(UniquenessType.node_global)
                .MaxDepth(2)
                .ToString();

            Console.WriteLine(qry);

            Assert.AreEqual(this.GetTestRestQuery(), qry);
        }

        [TestCase]
        public void REST_Traversal_Test()
        {
            Node node = Node.Get(19);
            Assert.IsNotNull(node);
            RestAPI r = new RestAPI();
            r.Order(OrderType.breadth_first)
                .Filter
                (
                    new PropertyFilter().SetPropertyName("FirstName").Contains("m")
                )
                .RelationShips
                (
                    new RelationShip()
                        .Add(new RelationFilter(RelationshipDirection.out_direction, "wife"))
                        .Add(new RelationFilter(RelationshipDirection.all_direction, "loves"))
                )
                .Uniqueness(UniquenessType.node_global)
                .MaxDepth(2);
            IList<Node> nodes = node.Filter(r);
            Assert.IsTrue(nodes.Count > 0);
        }
    }
}
