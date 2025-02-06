rs.initiate(
    {
        _id: "shard2rs",
        version: 1,
        members: [
            { _id: 0, host: "shard2-1:27017" },
            { _id: 1, host: "shard2-2:27017" },
        ]
    }
)