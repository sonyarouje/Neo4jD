using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public class Filter:RestBasePipe
    {
        private PropertyFilter _filter;
        public Filter(PropertyFilter propertyFilter)
        {
            _filter = propertyFilter;
        }

        public override object GetJsonObject()
        {
            return new JProperty("return_filter", _filter.GetJsonObject());
        }
    }
}
