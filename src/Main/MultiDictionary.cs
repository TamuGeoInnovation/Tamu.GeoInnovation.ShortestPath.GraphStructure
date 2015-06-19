using System.Collections.Generic;

namespace USC.GISResearchLab.ShortestPath.GraphStructure
{
    public class MultiDictionary<K1, K2, V>
    {
        private Dictionary<K1, Dictionary<K2, V>> list;

        public int Count { get { return list.Count; } }

        public MultiDictionary(int capacity)
        {
            list = new Dictionary<K1, Dictionary<K2, V>>(capacity);
        }

        public bool Contains(K1 key1, K2 key2)
        {
            return (list.ContainsKey(key1) && list[key1].ContainsKey(key2));
        }

        public V Get(K1 key1, K2 key2)
        {
            V value = default(V);
            if (Contains(key1, key2)) value = (list[key1])[key2];
            return value;
        }

        public void Set(K1 key1, K2 key2, V value)
        {
            Dictionary<K2, V> child = null;
            if (list.ContainsKey(key1))
            {
                child = list[key1];
                if (child.ContainsKey(key2)) child[key2] = value;
                else child.Add(key2, value);
            }
            else
            {
                child = new Dictionary<K2, V>(3);
                child.Add(key2, value);
                list.Add(key1, child);
            }
        }

        public void Remove(K1 key1, K2 key2)
        {
            if (list.ContainsKey(key1)) (list[key1]).Remove(key2);
        }
    }
}
