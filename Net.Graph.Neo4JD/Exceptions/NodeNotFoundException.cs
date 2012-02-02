using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Exceptions
{
    public class NodeNotFoundException:Exception
    {
        private readonly int _unknownNodeId;

        public NodeNotFoundException(Exception ex, int unknownNodeId)
        {
            if (ex.Message.Contains("404"))
                _unknownNodeId = unknownNodeId;
            else
                throw ex;
        }

        public override string Message
        {
            get
            {
                return string.Format("Cannot find node with id [{0}] in database.", _unknownNodeId.ToString());
            }
        }
    }
}
