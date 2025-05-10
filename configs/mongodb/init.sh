#!/bin/bash
echo "Running Mongo import script..."

until mongosh --eval "db.runCommand({ ping: 1 })"
do
    echo "Waiting for MongoDB to start..."
    sleep 2
done

mongoimport --db ecobay --collection Product --type json --file /seed/product.json --jsonArray

echo "Mongo import completed."