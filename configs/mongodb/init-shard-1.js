rs.initiate(
    {
        _id: "shard-01",
        version: 1,
        members: [
            { _id: 0, host: "shard01-1:27017" },
            { _id: 1, host: "shard01-2:27017" },
        ]
    }
)