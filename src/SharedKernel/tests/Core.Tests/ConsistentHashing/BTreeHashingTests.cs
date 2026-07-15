using Core.ConsistentHashing;

namespace Core.Tests.ConsistentHashing
{
    public class BTreeHashingTests
    {
        [Fact]
        public void IsInit_BeforeInit_ShouldReturnFalse()
        {
            var hashing = new BTreeHashing<string>();
            Assert.False(hashing.IsInit);
        }

        [Fact]
        public void Init_WithNodes_ShouldMarkAsInitialized()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1", "node2", "node3"]);
            Assert.True(hashing.IsInit);
        }

        [Fact]
        public void Init_CalledTwice_ShouldOnlyInitializeOnce()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1"]);
            hashing.Init(["node2", "node3"]); // second call should be ignored
            Assert.True(hashing.IsInit);

            // Since second Init was ignored, "node2"/"node3" virtual nodes should not exist;
            // GetBucket should still resolve without throwing.
            var bucket = hashing.GetBucket("some-key");
            Assert.NotNull(bucket);
        }

        [Fact]
        public void Init_WithReplicateCount_ShouldUseCustomReplicate()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1", "node2"], replicate: 10);
            Assert.True(hashing.IsInit);

            var bucket = hashing.GetBucket("test-key");
            Assert.NotNull(bucket);
        }

        [Fact]
        public void GetBucket_ShouldReturnConsistentResultForSameKey()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1", "node2", "node3"]);

            var bucket1 = hashing.GetBucket("stable-key");
            var bucket2 = hashing.GetBucket("stable-key");

            Assert.Equal(bucket1.Node, bucket2.Node);
        }

        [Fact]
        public void GetBucket_DifferentKeys_MayReturnDifferentNodes()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1", "node2", "node3"]);

            // With 3 nodes and 100 virtual nodes each, collecting enough keys
            // should eventually hit different nodes.
            var nodes = Enumerable.Range(0, 300)
                .Select(i => hashing.GetBucket($"key-{i}").Node)
                .Distinct()
                .ToList();

            Assert.True(nodes.Count > 1, "Multiple distinct nodes should be used across many keys.");
        }

        [Fact]
        public async Task GetBucketAsync_ShouldReturnSameResultAsGetBucket()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1", "node2", "node3"]);

            var sync = hashing.GetBucket("async-key");
            var async_ = await hashing.GetBucketAsync("async-key");

            Assert.Equal(sync.Node, async_.Node);
        }

        [Fact]
        public void Add_NewNode_ShouldBeResolvable()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1", "node2"]);
            hashing.Add("node3");

            var nodes = Enumerable.Range(0, 300)
                .Select(i => hashing.GetBucket($"k{i}").Node)
                .Distinct()
                .ToList();

            Assert.Contains("node3", nodes);
        }

        [Fact]
        public void Add_NullNode_ShouldThrowArgumentNullException()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1"]);
            Assert.Throws<ArgumentNullException>(() => hashing.Add(null!));
        }

        [Fact]
        public void Remove_ExistingNode_ShouldNoLongerBeResolved()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1", "node2", "node3"]);
            hashing.Remove("node3");

            var nodes = Enumerable.Range(0, 300)
                .Select(i => hashing.GetBucket($"k{i}").Node)
                .Distinct()
                .ToList();

            Assert.DoesNotContain("node3", nodes);
        }

        [Fact]
        public void Remove_NodeNotAdded_ShouldThrowException()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1"]);
            Assert.Throws<Exception>(() => hashing.Remove("node-never-added"));
        }

        [Fact]
        public void Remove_NullNode_ShouldThrowArgumentNullException()
        {
            var hashing = new BTreeHashing<string>();
            hashing.Init(["node1"]);
            Assert.Throws<ArgumentNullException>(() => hashing.Remove(null!));
        }

        [Fact]
        public void VirtualNode_Equals_SameIdAndNode_ShouldBeEqual()
        {
            var a = new VirtualNode<string>(1, "node1");
            var b = new VirtualNode<string>(1, "node1");
            Assert.True(a.Equals(b));
        }

        [Fact]
        public void VirtualNode_Equals_DifferentId_ShouldNotBeEqual()
        {
            var a = new VirtualNode<string>(1, "node1");
            var b = new VirtualNode<string>(2, "node1");
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void VirtualNode_Equals_Null_ShouldReturnFalse()
        {
            var a = new VirtualNode<string>(1, "node1");
            Assert.False(a.Equals(null));
        }
    }
}
