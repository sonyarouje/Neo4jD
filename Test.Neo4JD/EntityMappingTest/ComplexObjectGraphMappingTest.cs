using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using NUnit.Core;
using NUnit.Framework;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.EntityMapper;
namespace Test.Neo4jClient.EntityMappingTest
{
    public class Order
    {
        public Order()
        {
            _orderItems = new List<OrderItem>();
        }

        [EntityId]
        public int Id { get; set; }
        public virtual string Name { get; set; }

        IList<OrderItem> _orderItems;
        public virtual IList<OrderItem> OrderItems 
        { 
            get { return _orderItems; }
            private set { _orderItems = value; }
        }
        public void AddOrderItem(OrderItem item)
        {
            this._orderItems.Add(item);
        }
    }

    public class OrderItem
    {
        public OrderItem()
        {
        }
        public OrderItem(int id, Product product)
        {
            this.Id = id;
            this.Product = product;
        }
        [EntityId]
        public int Id { get; set; }

        public virtual Product Product { get; set; }
    }

    public class Product
    {
        public Product()
        {

        }
        public Product(int id, string productName)
        {
            this.Id = id;
            this.ProductName = productName;
        }
        [EntityId]
        public int Id { get; set; }
        public string ProductName { get; set; }
    }

    [TestFixture]
    public class ComplexObjectGraphMappingTest
    {
        private Order _order;

        [SetUp]
        public void Initialize()
        {
            GraphEnvironment.SetBaseUri("http://localhost:7474/");

            ModelBuilder.Add(new OrderConfiguration());
            ModelBuilder.Add(new OrderItemConfiguration());
            ModelBuilder.Add(new ProductConfiguration());
            _order = new Order();
            _order.Id = 0;
            _order.Name = "Sony";
            _order.AddOrderItem(new OrderItem(0, new Product(0, "Rice")));
            _order.AddOrderItem(new OrderItem(0, new Product(0, "Sugar")));
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ModelBuilder.Clear();
        }

        [TestCase]
        public void SaveObjectGraph()
        {
            NodeMapper nodeMapper = new NodeMapper();
            nodeMapper.Save<Order>(_order);
            Console.WriteLine(_order.Id.ToString());
            Assert.AreNotEqual(0, _order.Id);
        }

        [TestCase]
        public void GetComplexObject()
        {
            NodeMapper nodeMapper = new NodeMapper();
            Order order= nodeMapper.Get<Order>(14);
            Assert.AreEqual(14, order.Id);
            Assert.AreEqual(2, order.OrderItems.Count);
        }

        [TestCase]
        public void GetOrder()
        {
            NodeMapper nodeMapper = new NodeMapper();
            Order order = nodeMapper.Get<Order>(14);
            Assert.AreEqual(14, order.Id);
            foreach (OrderItem item in order.OrderItems)
            {
                Console.WriteLine(item.Id.ToString());
                Product prod = item.Product;
                if (prod != null)
                    Console.WriteLine(prod.ProductName);
            }
            Assert.AreEqual(2, order.OrderItems.Count);

        }
    }
}
