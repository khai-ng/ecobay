sh.addShard("shard1rs/shard1-1:27017")
sh.addShard("shard1rs/shard1-2:27017")

sh.addShard("shard2rs/shard2-1:27017")
sh.addShard("shard2rs/shard2-2:27017")

sh.enableSharding("ecobay")
db.adminCommand({ shardCollection: "ecobay.Product", key: { _id: "hashed" } })