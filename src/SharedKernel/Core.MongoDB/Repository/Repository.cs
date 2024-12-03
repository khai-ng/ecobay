using Core.Entities;
using Core.MongoDB.Context;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Core.MongoDB.Repository
{
    public abstract class Repository<TModel> : Repository<TModel, ObjectId>
        where TModel : AggregateRoot<ObjectId>
    {
        protected Repository(MongoContext context) : base(context)
        {
        }
    }

    public abstract class Repository<TModel, TKey> : IRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        private readonly MongoContext _mongoContext;
        private IMongoCollection<TModel> _collection => _mongoContext.Collection<TModel>();
        protected Repository(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            var data = await _collection.FindAsync(Builders<TModel>.Filter.Empty);
            return await data.ToListAsync();
        }

        public async Task<TModel?> FindAsync(TKey id)
        {
            var data = await _collection.FindAsync(Builders<TModel>.Filter.Eq("_id", id));
            return await data.SingleOrDefaultAsync();
        }

        public void Add(TModel entity)
            => _mongoContext.AddCommand(() => _collection.InsertOneAsync(entity));

        public void Update(TModel entity)
            => _mongoContext.AddCommand(() =>
                _collection.ReplaceOneAsync(
                    Builders<TModel>.Filter.Eq("_id", entity.Id),
                    entity
                ));

        public void Remove(TModel entity)
            => _mongoContext.AddCommand(() =>
                _collection.DeleteOneAsync(
                    Builders<TModel>.Filter.Eq("_id", entity.Id)
                ));

        public void AddRange(IEnumerable<TModel> entities)
            => _mongoContext.AddCommand(() => _collection.InsertManyAsync(entities));

        public void UpdateRange(IEnumerable<TModel> entities)
            => _mongoContext.AddCommand(() =>
            {
                List<Task> tasks = [];
                foreach (var item in entities)
                {
                    tasks.Add(_collection.ReplaceOneAsync(
                        Builders<TModel>.Filter.Eq("_id", item.Id),
                        item
                    ));
                }
                return Task.WhenAll(tasks);
            });

        public void RemoveRange(IEnumerable<TModel> entities)
            => _mongoContext.AddCommand(() =>
                _collection.DeleteManyAsync(
                    Builders<TModel>.Filter.In("_id", entities.Select(x => x.Id))
                )
            );
    }
}
