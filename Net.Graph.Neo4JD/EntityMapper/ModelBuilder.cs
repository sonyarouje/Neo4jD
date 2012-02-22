using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Graph.Neo4JD.EntityMapper
{
    public static class ModelBuilder
    {
        private static Dictionary<Type, EntityRelation> _relationShips = new Dictionary<Type, EntityRelation>();

        public static void Add(BaseConfiguration configuration)
        {
            _relationShips.Add(configuration.EntityType, configuration.GetRelatedEntityConfiguration());
        }

        public static IList<SubEntityDetails> GetSubEntities(Type parent)
        {
            if (_relationShips.ContainsKey(parent))
                return _relationShips[parent].DirectSubEntities;
            else
                //return new List<SubEntityDetails>();
                throw new Exception(string.Format("Unable to find entity {0} in model", parent.ToString()));
        }

        public static void Clear()
        {
            _relationShips.Clear();
        }
    }
}
