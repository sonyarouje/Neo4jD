using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public class FilterPipeExt : RestBasePipe
    {
        private Func<PropertyFilter, object> _filter;
        public FilterPipeExt(Func<PropertyFilter,object> property)
        {
            _filter = property;
        }

        public override object GetJsonObject()
        {
            //PropertyFilter tmp = (PropertyFilter)_filter(_filter.);
            return null;
            //return new JProperty("return_filter", _filter.GetJsonObject());
        }
    }
}
