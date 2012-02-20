using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD.EntityMapper;
namespace Test.Neo4jClient.EntityMappingTest
{
    public class OrderConfiguration:EntityConfiguration<Order>
    {
        public OrderConfiguration()
        {
            this.RelatedTo<OrderItem>(o => o.OrderItems);
        }
    }

    public class OrderItemConfiguration:EntityConfiguration<OrderItem>
    {
        public OrderItemConfiguration()
        {
            this.RelatedTo<Product>(oi => oi.Product);
        }
    }

    public class ProductConfiguration:EntityConfiguration<Product>
    {
        public ProductConfiguration()
        {
            this.Leaf();
        }
    }
}
