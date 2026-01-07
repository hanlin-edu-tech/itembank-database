using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Extensions;

public static class CreateIndexModelExtensions
{
    extension<T>(CreateIndexModel<T>) where T : class, IIndexable<T>
    {
        public static CreateIndexModel<T> Default()
        {
            return new CreateIndexModel<T>(
                Builders<T>.IndexKeys.Ascending("_id"),
                new CreateIndexOptions { Name = "_id_" }
            );
        }
    }
}