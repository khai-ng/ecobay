using System.IO.Hashing;
using System.Text;

namespace Core.ConsistentHashing
{
    public class BTreeHashing<T>
    {
        private int _replicate = 100;
        private int[] ringKeys = [];

        private readonly SortedDictionary<int, VirtualNode<T>> hashRing = [];
        public bool IsInit => hashRing.Count > 0;

        public void Init(IEnumerable<T> nodes)
        {
            if (IsInit)
                return;

            foreach (T node in nodes)
            {
                AddNode(node);
            }
            ringKeys = [.. hashRing.Keys];
        }

        public void Init(IEnumerable<T> nodes, int replicate)
        {
            if (IsInit)
                return;

            _replicate = replicate;
            Init(nodes);
        }

        public IEnumerable<VirtualNode<T>> Add(T node)
        {
            ArgumentNullException.ThrowIfNull(node);
            List<VirtualNode<T>> virtualNode = [];
            for (int i = 0; i < _replicate; i++)
            {
                int hash = Hash(node.GetHashCode().ToString() + i);
                hashRing[hash] = new VirtualNode<T>(i, node);
                var next = Lockup(ringKeys, hash);
                virtualNode.Add(hashRing[ringKeys[next]]);
            }

            ringKeys = [.. hashRing.Keys];
            return virtualNode.Distinct();
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
            ringKeys = [.. hashRing.Keys];
        }

        public VirtualNode<T> GetBucket(string key)
        {
            int hash = Hash(key);
            var next = Lockup(ringKeys, hash);

            return hashRing[ringKeys[next]];
        }

        private void AddNode(T node)
        {
            ArgumentNullException.ThrowIfNull(node);
            for (int i = 0; i < _replicate; i++)
            {
                int hash = Hash(node.GetHashCode().ToString() + i);
                hashRing[hash] = new VirtualNode<T>(i, node);
            }
        }

        private IEnumerable<VirtualNode<T>> AddToUpdate(T node)
        {
            ArgumentNullException.ThrowIfNull(node);
            List<VirtualNode<T>> updatingNode = []; 
            for (int i = 0; i < _replicate; i++)
            {
                int hash = Hash(node.GetHashCode().ToString() + i);
                hashRing[hash] = new VirtualNode<T>(i, node);
                var next = Lockup(ringKeys, hash);
                updatingNode.Add(hashRing[ringKeys[next]]);
            }
            return updatingNode.Distinct();
        }

        //return the index of first item that >= val.
        //if not exist, return 0;
        //ring should be ordered array.
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
            return  end;
        }

        private static int Hash(string key)
        {
            var hash = XxHash32.HashToUInt32(Encoding.ASCII.GetBytes(key));
            return (int)hash;
        }
    }

    public class VirtualNode<T>: IEquatable<VirtualNode<T>>
    {
        public int VirtualId { get; set; }
        public T Node { get; set; }
        public VirtualNode(int replicateId, T node) { VirtualId = replicateId; Node = node; }

        public bool Equals(VirtualNode<T>? other)
        {
            return VirtualId == other?.VirtualId 
                && Node != null && Node.Equals(other.Node);
        }
    }
}
