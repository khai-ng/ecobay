using Core.MongoDB.Context;
using Core.Repository;
using Core.SharedKernel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Core.MongoDB.Repository
{
    public abstract class Repository<TModel> : Repository<TModel, ObjectId>
        where TModel : AggregateRoot<ObjectId>
    {
        protected Repository(IMongoContext context) : base(context)
        {
        }
    }

    public abstract class Repository<TModel, TKey> :
        IRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        private readonly IMongoContext _mongoContext;
        protected Repository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        protected virtual IMongoCollection<TModel> Collection => _mongoContext.GetCollection<TModel>(nameof(TModel));
        public IMongoCollection<TModel> DbSet => Collection;

        //public  => _collection;

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            var data = await Collection.FindAsync(Builders<TModel>.Filter.Empty);
            return await data.ToListAsync();
        }

        public async Task<TModel?> FindAsync(TKey id)
        {
            var data = await Collection.FindAsync(Builders<TModel>.Filter.Eq("_id", id));
            return await data.SingleOrDefaultAsync();
        }

        public void AddRange(IEnumerable<TModel> entities)
            => _mongoContext.AddCommand(() => Collection.InsertManyAsync(entities));

        public void UpdateRange(IEnumerable<TModel> entities)
            => _mongoContext.AddCommand(() =>
            {
                List<Task> tasks = [];
                foreach (var item in entities)
                {
                    tasks.Add(Collection.ReplaceOneAsync(
                        Builders<TModel>.Filter.Eq("_id", item.Id),
                        item
                    ));
                }
                return Task.WhenAll(tasks);
            });

        public void RemoveRange(IEnumerable<TModel> entities)
            => _mongoContext.AddCommand(() =>
                Collection.DeleteManyAsync(
                    Builders<TModel>.Filter.In("_id", entities.Select(x => x.Id))
                )
            );
    }
}
