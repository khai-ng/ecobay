Microservice - Clean Architecture template

sharding mongodb:

docker-compose exec configsvr01 sh -c "mongosh < /scripts/init-configserver.js"

docker-compose exec shard01-1 sh -c "mongosh < /scripts/init-shard-1.js"
docker-compose exec shard02-1 sh -c "mongosh < /scripts/init-shard-2.js"

docker-compose exec router01 sh -c "mongosh < /scripts/init-router.js"


docker-compose exec router01 mongosh --port 27017
sh.enableSharding("ecobay")
db.adminCommand( { shardCollection: "ecobay.Product", key: { _id: "hashed" } } )
sh.status()

use ecobay
db.stats()
db.Product.getShardDistribution()