using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public class PropertyFilter
    {
        private string _propertyName=string.Empty;
        private string _propertyValue = string.Empty;
        private string _selectCriteria = string.Empty;

        public PropertyFilter SetPropertyName(string propertyName)
        {
            _propertyName = propertyName;
            return this;
        }

        public PropertyFilter Contains(string propertyValue)
        {
            _propertyValue = propertyValue;
            _selectCriteria = "contains";
            return this;
        }

        public PropertyFilter Equals(string propertyValue)
        {
            _propertyValue = propertyValue;
            _selectCriteria = "equals";
            return this;
        }

        internal object GetJsonObject()
        {
            JObject returnFilter = new JObject();
            string filter = string.Format("position.endNode().getProperty('{0}').toLowerCase().{1}('{2}')",_propertyName,_selectCriteria,_propertyValue);
            returnFilter.Add ("body", new JValue(filter));
            returnFilter.Add("language", new JValue("javascript"));
            return returnFilter;
        }
    }
}
