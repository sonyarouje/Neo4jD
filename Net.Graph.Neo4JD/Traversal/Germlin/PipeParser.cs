using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace Net.Graph.Neo4JD.Traversal.Germlin
{
    public class PipeParser
    {
        public static string Parse(IList<Pipes.Pipe> pipes)
        {
            StringBuilder builder = new StringBuilder();
            string seperator = string.Empty;
            foreach (Pipes.Pipe pipe in pipes)
            {
                builder.Append(seperator);
                builder.Append(pipe.ToString());

                if (string.IsNullOrEmpty(seperator))
                    seperator = ".";
            }
            builder.Append(";");
            JObject jobject = new JObject();
            jobject.Add("script", new JValue(builder.ToString()));
            return jobject.ToString();
        }
    }
}
