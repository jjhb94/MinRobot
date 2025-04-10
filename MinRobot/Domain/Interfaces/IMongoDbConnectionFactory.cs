namespace MinRobot.Infrastructure.Repository;

public abstract class MongoRepositoryBase<T>
{
    protected readonly IMongoCollection<T> _collection;

    protected MongoRepositoryBase(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }
}