using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
namespace Net.Graph.Neo4JD.Persistance
{
    public class RequestResult
    {
        private WebResponse _response=null;
        public RequestResult(WebResponse response)
        {
            _response = response;
        }

        public Uri GetLocation()
        {
            return new Uri(_response.Headers["Location"]);
        }

        public string GetResponseData()
        {
            var readStream = new StreamReader(_response.GetResponseStream(), Encoding.UTF8);
            var buffer = new Char[256];

            var stringBuilder = new StringBuilder();
            int count = readStream.Read(buffer, 0, 256);
            while (count > 0)
            {
                var str = new String(buffer, 0, count);
                stringBuilder.Append(str);
                count = readStream.Read(buffer, 0, 256);
            }
            var responseData = stringBuilder.ToString();

            Console.WriteLine(responseData);

            return responseData;
        }

    }
}
