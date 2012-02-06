using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public enum OrderType
    {
        breadth_first=0,
        depth_first =1
    }
    public class Order:RestBasePipe
    {
        public Order(OrderType searchType)
        {
            base.SetPipeName("order");
            base.SetPipeValue(searchType.ToString());
        }

        public override object GetJsonObject()
        {
            return new JProperty(base.GetPipeName(), base.GetPipeValue());
        }
    }
}
