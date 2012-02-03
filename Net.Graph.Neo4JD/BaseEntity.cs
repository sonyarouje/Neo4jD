using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Net.Graph.Neo4JD.Persistance;
namespace Net.Graph.Neo4JD
{
    public class BaseEntity
    {
        protected Dictionary<string, object> _keyValuePair = new Dictionary<string, object>();
        private Uri _location = null;
        private Repository _baseRepo;
        public BaseEntity()
        {
            
        }

        protected void SetRepository(Repository repository)
        {
            _baseRepo = repository;
        }

        public int Id { get { return GetId(); } }
        private int GetId()
        {
            if (_location == null)
                return 0;
            string[] locationAsArr = _location.ToString().Split("/".ToCharArray());
            return Convert.ToInt32(locationAsArr[locationAsArr.Length - 1]);
        }

        public virtual BaseEntity SetProperty(string propertyName, string propertyValue)
        {
            if (this.GetLocation() == null || this.Id <= 0)
                throw new Exceptions.InvalidNodeException();
            this.AddProperty(propertyName, propertyValue);
            return _baseRepo.SetProperty(this, propertyName);
        }

        /// <summary>
        /// This function will replace all existing properties on the node with the new set of Property.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public virtual BaseEntity UpdateProperties(string propertyName, string propertyValue)
        {
            if (this.GetLocation() == null || this.Id <= 0)
                throw new Exceptions.InvalidNodeException();
            this._keyValuePair[propertyName] = propertyValue;
            JObject props = new JObject();
            props.Add(propertyName, new JValue(propertyValue));
            _baseRepo.UpdateProperty(this, props.ToString());
            return this;
        }

        /// <summary>
        /// Properties added through this function will get persisted when the Node/Relationship 
        /// calls Create function.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public BaseEntity AddProperty(string propertyName, string propertyValue)
        {
            this._keyValuePair.Add(propertyName, propertyValue);
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
