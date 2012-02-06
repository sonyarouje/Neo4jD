using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core;
using NUnit.Framework;
using Net.Graph.Neo4JD.Traversal.Germlin;
namespace Test.Neo4jClient
{
    [TestFixture]
    public class GermlinLanguageTest
    {
        [TestCase]
        public void BasicGermlinParse()
        {
            GermlinPipe germlinPipe=new GermlinPipe();
            string parsedString = germlinPipe.G().V(30).Out("sony").In("arouje").Filter("FName", new Net.Graph.Neo4JD.Traversal.Germlin.Pipes.FilterPipe("Kevin")).ToString();

            Assert.AreEqual("g.V(30).out('sony').in('arouje').filter{it.FName=='Kevin'}", parsedString);
        }
    }
}
