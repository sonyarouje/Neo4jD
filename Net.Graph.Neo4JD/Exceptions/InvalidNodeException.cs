using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Exceptions
{
    public class InvalidNodeException:Exception
    {
        public override string Message
        {
            get
            {
                return "Location is null. Get a valid Node/Relationship from db to perform this operation.";
            }
        }
    }
}
