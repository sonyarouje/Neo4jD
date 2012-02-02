using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Exceptions
{
    public class NodeDeleteException:Exception
    {
        private readonly Node _nodeDeleteFailed;
        public NodeDeleteException(Node nodeDeleteFailed)
        {
            _nodeDeleteFailed = nodeDeleteFailed;
        }

        public override string Message
        {
            get
            {
                return string.Format("The node with id {0} cannot be deleted. Check that the node is orphaned before deletion.", _nodeDeleteFailed.Id.ToString());
            }
        }
    }
}
