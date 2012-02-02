using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace Net.Graph.Neo4J.Client
{
    public class HttpHelper
    {
        public static Uri ConcatUri(Uri uri, params object[] uriParts)
        {
            var stringBuilder = new StringBuilder();
            foreach (var uriPart in uriParts)
            {
                stringBuilder.Append(uriPart).Append("/");
            }

            var toAdd = stringBuilder.ToString().TrimEnd('/');

            return new Uri(string.Concat(uri.AbsoluteUri, toAdd));
        }

        public static void AddBody(HttpWebRequest request, string nodeData)
        {
            request.ContentType = "application/json";

            byte[] bytes = Encoding.UTF8.GetBytes(nodeData);
            request.ContentLength = bytes.Length;

            using (var reqStream = request.GetRequestStream())
            {
                reqStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
