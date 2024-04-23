using Lumbre.Interfaces.Common;
using Lumbre.Persistance.Mongodb.Configuration;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Lumbre.Interfaces.Repository;

namespace Lumbre.Persistance.Mongodb.Implementation
{
    /// <summary>
    /// Implements a MongoDB Repository
    /// </summary>
    internal class MongoRepository : IRepository
    {
        private readonly RespositoryConfiguration _configuration;

        public const string EntityId = "id";
        public const string MongoDBId = "_id";

        public MongoRepository(RespositoryConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IRepositoryResult> ReadById(Primitives.CollectionName collectionName, Primitives.ResourceId resourceId)
        {
            var client = CreateClient();

            var collection = client
                                .GetDatabase(_configuration.DatabaseName)
                                .GetCollection<BsonDocument>(collectionName);


            var filter = Builders<BsonDocument>.Filter.Eq(EntityId, resourceId.Id);
            var projection = Builders<BsonDocument>.Projection.Exclude(MongoDBId); // avoids introducing extrenous elements on the resulting payload

            var cursor = await collection
                .Aggregate<BsonDocument>()
                .Match(filter)
                .Project(projection)
                .ToListAsync();


            var value = cursor
                .FirstOrDefault();

            if (value == null)
            {
                return new NotFound();
            }

            var jsDoc = BsonTypeMapper.MapToDotNetValue(value);
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(jsDoc);

            return new Results(new Primitives.JsonPayload(str));
        }

        public Task Upsert(Primitives.CollectionName collection, Primitives.ResourceId resourceId, Primitives.JsonPayload payload)
        {
            throw new NotImplementedException();
        }

        internal MongoClient CreateClient()
        {
            return new MongoClient(_configuration.ConnectionString);
        }
    }
}
