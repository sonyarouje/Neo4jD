using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Castle.DynamicProxy;
namespace Net.Graph.Neo4JD.EntityMapper
{
    internal class ParentChildGlue
    {
        private readonly object _parent;
        private readonly object _child;
        private readonly string _memberName;
        public ParentChildGlue(object parent, object child, string memberName)
        {
            _parent = parent;
            _child = child;
            _memberName = memberName;
        }

        public object GetParent()
        {
            return _parent;
        }

        public object GetChild()
        {
            return _child;
        }

        public string GetMemeberName()
        {
            return _memberName;
        }
    }

    internal class RelationshipCreateHelper
    {
        private NodeMapper _nodeMapper;
        Stack<ParentChildGlue> _toWalk = new Stack<ParentChildGlue>();

        public RelationshipCreateHelper(NodeMapper nodeMapper)
        {
            _nodeMapper = nodeMapper;
        }

        public object SaveNodeWithRelationShip(object proxy)
        {
            object entity = ProxyCloner.ConvertProxyToEntity(proxy);
            ParentChildGlue parentRelation = new ParentChildGlue(null, entity, string.Empty);
            _toWalk.Push(parentRelation);
            PersistObjectGraphWithRelationship(_toWalk.Pop());
            return entity;
        }

        private void PersistObjectGraphWithRelationship(ParentChildGlue parentRelation)
        {
            object entityParent = parentRelation.GetParent();
            object entity = parentRelation.GetChild();
            Node parent = null;

            Node child = _nodeMapper.SaveAndReturnAsNode(entity);
            if (entityParent != null)
            {
                parent = _nodeMapper.SaveAndReturnAsNode(entityParent);
                parent.CreateRelationshipTo(child, parentRelation.GetMemeberName());
            }

            this.QueueDirectChild(entity);

            if (_toWalk.Count == 0) return;
            PersistObjectGraphWithRelationship(_toWalk.Pop());
        }

        private void QueueDirectChild(object entity)
        {
            IList<SubEntityDetails> entitDetails = ModelBuilder.GetSubEntities(entity.GetType());

            foreach (SubEntityDetails subEntity in entitDetails)
            {
                entity.GetType().GetProperties().Where(pr => pr.CanRead).ToList().ForEach(property =>
                {
                    if (property.Name == subEntity.MemberName)
                    {
                        if (subEntity.IsCollection)
                        {
                            object obj = ((IEnumerable)property.GetValue(entity, null)).Cast<object>().ToArray();
                            Schedule(obj, entity, subEntity.MemberName);
                        }
                        else
                            Schedule(property.GetValue(entity, null), entity, subEntity.MemberName);

                    }
                });

            }
        }

        private void Schedule(Object toSchedule, object parent, string memberName)
        {
            if (toSchedule == null) return;

            if (toSchedule.GetType().IsArray || toSchedule.GetType() == typeof(IList))
                foreach (Object item in ((Array)toSchedule))
                    Schedule(item, parent, memberName);
            else
            {
                object entityToSchedule = ProxyCloner.ConvertProxyToEntity(toSchedule);
                ParentChildGlue parentRelation = new ParentChildGlue(parent, entityToSchedule, memberName);
                _toWalk.Push(parentRelation);
            }
        }

        
    }
}
