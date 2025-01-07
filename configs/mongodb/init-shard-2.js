rs.initiate(
    {
        _id: "shard-02",
        version: 1,
        members: [
            { _id: 0, host: "shard02-1:27017" },
            { _id: 1, host: "shard02-2:27017" },
        ]
    }
)