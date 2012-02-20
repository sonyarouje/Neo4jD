using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
namespace Net.Graph.Neo4JD.EntityMapper
{
    public sealed class ObjectWalker : IEnumerable, IEnumerator
    {
        private Object m_current;
        private Stack m_toWalk = new Stack();
        private ObjectIDGenerator m_idGen = new ObjectIDGenerator();

        public ObjectWalker(Object root)
        {
            Schedule(root,null);
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public void Reset()
        {
            throw new NotSupportedException("Resetting the enumerator is not supported.");
        }

        public Object Current { get { return m_current; } }

        private void Schedule(Object toSchedule, object parent)
        {
            if (toSchedule == null) return;

            Boolean firstOccurrence;
            m_idGen.GetId(toSchedule, out firstOccurrence);
            if (!firstOccurrence) return;

            if (toSchedule.GetType().IsArray || toSchedule.GetType()==typeof(IList))
                foreach (Object item in ((Array)toSchedule))
                {
                    Schedule(item, null);
                }
            else
                m_toWalk.Push(toSchedule);
        }

        public Boolean MoveNext()
        {
            if (m_toWalk.Count == 0) return false;
            m_current = m_toWalk.Pop();
            foreach (PropertyInfo info in m_current.GetType().GetProperties())
            {
                if (MapperHelper.IsPrimitive(info.PropertyType) == false)
                {
                    if (info.PropertyType.Namespace == "System.Collections.Generic")
                    {
                        object obj = ((IEnumerable)info.GetValue(m_current, null)).Cast<object>().ToArray(); //((ICollection)info.GetValue(m_current, null));
                        Schedule(obj, m_current);
                    }
                    else
                        Schedule(info.GetValue(m_current, null),null);
                }
            }

            return true;
        }

        private Boolean IsTerminalObject(Object data)
        {
            Type t = data.GetType();
            return t.IsPrimitive || t.IsEnum || t.IsPointer || data is String;
        }
    }
}