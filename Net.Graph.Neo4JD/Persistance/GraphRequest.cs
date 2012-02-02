using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Net.Graph.Neo4JD;
namespace Net.Graph.Neo4JD.Persistance
{
    public class GraphRequest
    {
        private Uri _uriToRequest;
        
        public RequestResult Post(string method, Uri uri, string properties)
        {
            if (uri == null) throw new ArgumentNullException("uri should not be null");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Method = method;
            if (!string.IsNullOrWhiteSpace(properties))
                this.AddBody(httpWebRequest, properties);
            var response = httpWebRequest.GetResponse();
            RequestResult result = new RequestResult(response);
            return result;
        }



        private void AddBody(HttpWebRequest request, string nodeData)
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
