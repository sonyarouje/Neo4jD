using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.Persistance;
namespace Net.Graph.Neo4JD.JsonParser
{
    public class ParserBase
    {
        Dictionary<string, object> _keyValuePair;
        private string _responseData = string.Empty;
        public ParserBase(Dictionary<string,object> keyValuePair)
        {
            _keyValuePair = keyValuePair;
        }

        protected string GetResponseData()
        {
            return _responseData;
        }
        public virtual void JsonToEntity(RequestResult result, BaseEntity entity)
        {
            _responseData = result.GetResponseData();
            JObject jobject = JObject.Parse(_responseData);
            JToken token;
            jobject.TryGetValue("data", out token);
            var dataKeys = from p in jobject["data"] select p;

            IList<string> keys = jobject.Properties().Select(p => p.Name).ToList();
            foreach (var t in token)
            {
                string[] s = t.ToString().Replace("\"", "").Split(":".ToCharArray());
                this._keyValuePair.Add(s[0].Trim(), s[1].Trim());
            }
            entity.SetLocation(new Uri(jobject["self"].ToString()));
        }

        public virtual string EntityToJson()
        {
            JObject props = new JObject();
            foreach (var key in _keyValuePair.Keys)
            {
                props.Add(key, new JValue(_keyValuePair[key]));
            }
            return props.ToString();
        }

        
    }
}
