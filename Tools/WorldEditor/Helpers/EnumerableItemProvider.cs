using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldEditor.Helpers.Collections;

namespace WorldEditor.Helpers
{
    public class EnumerableItemProvider<T> : IItemsProvider<T>
    {
        private readonly IEnumerator<T> m_enumerator;
        private bool m_isEnded;
        private int m_count = 0;
        public bool IsEnded
        {
            get
            {
                return m_isEnded;
            }
        }
        public EnumerableItemProvider(IEnumerable<T> enumerable)
        {
            m_enumerator = enumerable.GetEnumerator();
        }
        public int FetchCount()
        {
            return m_isEnded ? m_count : (m_count + 1);
        }
        public IList<T> FetchRange(int startIndex, int count)
        {
            IList<T> result;
            if (m_isEnded && m_count == 0)
            {
                result = new List<T>();
            }
            else
            {
                if (m_isEnded || startIndex > m_count)
                {
                    m_enumerator.Reset();
                    m_count = 0;
                }
                List<T> list = new List<T>();
                bool movenext = true;
                while (startIndex + count > m_count && (movenext = m_enumerator.MoveNext()))
                {
                    if (startIndex <= m_count)
                    {
                        list.Add(m_enumerator.Current);
                    }
                    m_count++;
                }
                if (!movenext)
                {
                    m_isEnded = true;
                }
                result = list;
            }
            return result;
        }
    }
}
