using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.Traversal.Germlin;
using Newtonsoft.Json.Linq;
namespace Test.Neo4jClient
{
    [TestFixture]
    public class GermlinLanguageTest
    {
        [SetUp]
        public void Initialize()
        {
            GraphEnvironment.SetBaseUri("http://localhost:7474/");
        }

        [TestCase]
        public void BasicGermlinParse()
        {
            GermlinPipe germlinPipe=new GermlinPipe();
            string parsedString = germlinPipe.G.V.Out("knows").In("family").Filter("FName", new Net.Graph.Neo4JD.Traversal.Germlin.Pipes.FilterPipe("Kevin")).ToString();
            JObject jobject =new JObject();
            jobject.Add("script", new JValue("g.v(#id).out('knows').in('family').filter{it.FName=='Kevin'}"));
            Assert.AreEqual(jobject.ToString(), parsedString);

        }

        [TestCase]
        public void Get_Out_Nodes()
        {
            GermlinPipe germlinQuery = new GermlinPipe();
            germlinQuery.G.V.Out("son");
            Console.WriteLine(germlinQuery.ToString());
            Node father = Node.Get(1);
            IList<Node> nodes = father.Filter(germlinQuery);
            Assert.AreEqual(1, nodes.Count);
        }
    }
}
