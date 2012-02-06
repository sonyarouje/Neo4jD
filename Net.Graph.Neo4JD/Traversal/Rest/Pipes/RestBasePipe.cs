using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Rest.Pipes
{
    public abstract class RestBasePipe
    {
        private string _pipeName;
        private object _value;

        protected void SetPipeName(string pipeName)
        {
            _pipeName = pipeName;
        }

        protected void SetPipeValue(object pipeValue)
        {
            _value = pipeValue;
        }

        public virtual object GetJsonObject()
        {
            return null;
        }

        public string GetPipeName()
        {
            return _pipeName;
        }

        public string GetPipeValue()
        {
            return _value.ToString();
        }

        public override string ToString()
        {
            JObject props = new JObject();
            props.Add(_pipeName, new JValue(_value));
            return props.ToString();
        }
    }
}
