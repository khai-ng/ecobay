Microservice - Clean Architecture template

sharding mongodb:

docker-compose exec configsvr1 sh -c "mongosh < /scripts/init-configserver.js"

docker-compose exec shard1-1 sh -c "mongosh < /scripts/init-shard-1.js"
docker-compose exec shard2-1 sh -c "mongosh < /scripts/init-shard-2.js"

docker-compose exec router1 sh -c "mongosh < /scripts/init-router.js"


docker-compose exec router1 mongosh --port 27017
sh.enableSharding("ecobay")
db.adminCommand( { shardCollection: "ecobay.Product", key: { _id: "hashed" } } )
sh.status()

use ecobay
db.stats()
db.Product.getShardDistribution()