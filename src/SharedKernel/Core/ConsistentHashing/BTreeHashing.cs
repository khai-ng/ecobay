using System.IO.Hashing;
using System.Text;

namespace Core.ConsistentHashing
{
    public class BTreeHashing<T>
    {
        private int _replicate = 100;
        private int[] ringKeys = [];

        public SortedDictionary<int, T> hashRing = [];
        public bool IsInit => hashRing.Count > 0;

        public void Init(IEnumerable<T> nodes)
        {
            if (IsInit)
                return;

            foreach (T node in nodes)
            {
                Add(node, false);
            }
            ringKeys = hashRing.Keys.ToArray();
        }

        public void Init(IEnumerable<T> nodes, int replicate)
        {
            if (IsInit)
                return;

            _replicate = replicate;
            Init(nodes);
        }

        public void Add(T node)
        {
            ArgumentNullException.ThrowIfNull(node);
            Add(node, true);
        }

        public void Remove(T node)
        {
            ArgumentNullException.ThrowIfNull(node);
            for (int i = 0; i < _replicate; i++)
            {
                int hash = Hash(node.GetHashCode().ToString() + i);
                if (!hashRing.Remove(hash))
                {
                    throw new Exception("can not remove a node that not added");
                }
            }
            ringKeys = hashRing.Keys.ToArray();
        }

        public T GetBucket(string key)
        {
            int hash = Hash(key);
            int first = Lockup(ringKeys, hash);

            return hashRing[ringKeys[first]];
        }

        private void Add(T node, bool updateKeyArray)
        {
            for (int i = 0; i < _replicate; i++)
            {
                int hash = Hash(node.GetHashCode().ToString() + i);
                hashRing[hash] = node;
            }

            if (updateKeyArray)
                ringKeys = hashRing.Keys.ToArray();
        }

        //return the index of first item that >= val.
        //if not exist, return 0;
        //ay should be ordered array.
        private int Lockup(int[] ring, int value)
        {
            int begin = 0;
            int end = ring.Length - 1;
            int mid;

            if (ring[end] < value || ring[0] > value)
                return 0;

            while (end - begin > 1)
            {
                mid = (end + begin) / 2;
                if (ring[mid] >= value)
                    end = mid;
                else
                    begin = mid;
            }

            if (ring[begin] > value || ring[end] < value)
                throw new Exception("should not happen");

            return end;
        }

        private static int Hash(string key)
        {
            var hash = XxHash32.HashToUInt32(Encoding.ASCII.GetBytes(key));
            return (int)hash;
        }
    }
}
