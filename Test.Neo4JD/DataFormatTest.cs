using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
namespace Test.Neo4jClient
{
    [TestFixture]
    public class DataFormatTest
    {
        [TestCase]
        public void FormatPropertyInJson()
        {
            JObject query = new JObject();
            JProperty orderProp = new JProperty("order", "breadth_first");
            query.Add(orderProp);

            JObject returnFilter = new JObject();
            returnFilter.Add("body", new JValue("position.endNode().getProperty('name').toLowerCase().contains('t')"));
            returnFilter.Add("language", new JValue("javascript"));

            query.Add("return_filter", new JValue(returnFilter.ToString()));

            JObject pruneEval = new JObject();
            pruneEval.Add("body", new JValue("position.length() > 10"));
            pruneEval.Add("language", new JValue("javascript"));
            query.Add("prune_evaluator", pruneEval.ToString());

            query.Add("uniqueness", new JValue("node_global"));

            JArray relationships = new JArray();
            JObject relationShip1 = new JObject();
            relationShip1.Add("direction", new JValue("all"));
            relationShip1.Add("type", new JValue("knows"));
            relationships.Add(relationShip1);

            JObject relationShip2 = new JObject();
            relationShip2.Add("direction", new JValue("all"));
            relationShip2.Add("type", new JValue("loves"));
            relationships.Add(relationShip2);

            query.Add("relationships", relationships.ToString());
            //arr.Add(
            Console.WriteLine(query.ToString());
            //Assert.AreEqual(@"""order"" : ""breadth_first""", jobject.ToString());
        }
    }
}
