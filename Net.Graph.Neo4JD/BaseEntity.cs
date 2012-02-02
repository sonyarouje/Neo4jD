using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD
{
    public class BaseEntity
    {
        protected Dictionary<string, object> _keyValuePair = new Dictionary<string, object>();
        private Uri _location = null;

        public BaseEntity()
        {
        }

        public int Id { get { return GetId(); } }
        private int GetId()
        {
            if (_location == null)
                return 0;
            string[] locationAsArr = _location.ToString().Split("/".ToCharArray());
            return Convert.ToInt32(locationAsArr[locationAsArr.Length - 1]);
        }
        public BaseEntity SetProperty(string key, string value)
        {
            this._keyValuePair.Add(key, value);
            return this;
        }

        public string GetProperty(string key)
        {
            if (this._keyValuePair.ContainsKey(key) == false)
                return string.Empty;

            return this._keyValuePair[key].ToString();
        }


        public virtual BaseEntity Create()
        {
            return this;
        }

        public virtual void Delete()
        {

        }

        internal void SetLocation(Uri location)
        {
            _location = location;
        }

        public Uri GetLocation()
        {
            return _location;
        }

        public virtual string GetProperties()
        {
            JsonParser.NodeParser nodeParser = new JsonParser.NodeParser(this._keyValuePair);
            return nodeParser.EntityToJson();
        }

        internal int Count
        {
            get { return _keyValuePair.Count; }
        }
    }
}
