using Core.Entities;
using Core.MongoDB.Context;
using Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.MongoDB.Repository
{
    public abstract class Repository<TEntity> : Repository<TEntity, ObjectId>
        where TEntity : AggregateRoot<ObjectId>
    {
        protected Repository(MongoContext context) : base(context)
        {
        }
    }

    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : AggregateRoot<TKey>
    {
        private readonly MongoContext _mongoContext;
        private IMongoCollection<TEntity> _collection => _mongoContext.Collection<TEntity>();
        protected Repository(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var data = _collection
                .Find(Builders<TEntity>.Filter.Empty);

            return await data
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TDestination>> GetAllAsync<TDestination>(Expression<Func<TEntity, TDestination>> selector)
        {
            var data = _collection
                .Find(Builders<TEntity>.Filter.Empty)
                .Project(selector);

            return await data
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<TEntity?> FindAsync(TKey id)
        {
            var data = _collection
                .Find(Builders<TEntity>.Filter.Eq("_id", id));

            return await data
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<TDestination?> FindAsync<TDestination>(TKey id, Expression<Func<TEntity, TDestination>> selector)
        {
            var data = _collection
                .Find(Builders<TEntity>.Filter.Eq("_id", id))
                .Project(selector);

            return await data
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public void Add(TEntity entity)
            => _mongoContext.AddCommand(() => _collection.InsertOneAsync(entity));

        public void Update(TEntity entity)
            => _mongoContext.AddCommand(() =>
                _collection.ReplaceOneAsync(
                    Builders<TEntity>.Filter.Eq("_id", entity.Id),
                    entity
                ));

        public void Remove(TEntity entity)
            => _mongoContext.AddCommand(() =>
                _collection.DeleteOneAsync(
                    Builders<TEntity>.Filter.Eq("_id", entity.Id)
                ));

        public void AddRange(IEnumerable<TEntity> entities)
            => _mongoContext.AddCommand(() => _collection.InsertManyAsync(entities));

        public void UpdateRange(IEnumerable<TEntity> entities)
            => _mongoContext.AddCommand(() =>
            {
                List<Task> tasks = [];
                foreach (var item in entities)
                {
                    tasks.Add(_collection.ReplaceOneAsync(
                        Builders<TEntity>.Filter.Eq("_id", item.Id),
                        item
                    ));
                }
                return Task.WhenAll(tasks);
            });

        public void RemoveRange(IEnumerable<TEntity> entities)
            => _mongoContext.AddCommand(() =>
                _collection.DeleteManyAsync(
                    Builders<TEntity>.Filter.In("_id", entities.Select(x => x.Id))
                )
            );
    }
}
