using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Core;
using Net.Graph.Neo4JD.EntityMapper;
namespace Test.Neo4jClient.EntityMappingTest
{


    [TestFixture]
    public class ObjectGraphTraversal_Test
    {
        private Order _order;
        [SetUp]
        public void Initialize()
        {
            _order = new Order();
            _order.Id = 10;
            _order.AddOrderItem(new OrderItem(12, new Product(2, "Rice")));
            _order.AddOrderItem(new OrderItem(13, new Product(3, "Sugar")));
        }

        [TestCase]
        public void WalkObject()
        {
            ObjectWalker walker = new ObjectWalker(_order);
            int num = 0;
            foreach (object o in walker)
            {
                Console.WriteLine("Object #{0}: Type={1}, Value's string={2}", num++, o.GetType(), o.ToString());
            }
        }
    }
}
