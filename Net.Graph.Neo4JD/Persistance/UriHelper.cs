using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.Persistance
{
    public static class UriHelper
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
    }
}
