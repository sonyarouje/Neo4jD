using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Traversal.Germlin.Pipes
{
    public abstract class Pipe
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
            if (_value==null)
                return _pipeName;

            if(_value.GetType()==typeof(int))
                return string.Format("{0}({1})", _pipeName, _value);
  
            return string.Format("{0}('{1}')", _pipeName, _value);
        }
    }
}
