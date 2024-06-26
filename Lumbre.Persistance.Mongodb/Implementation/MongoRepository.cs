﻿using Lumbre.Interfaces.Common;
using Lumbre.Persistance.Mongodb.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using Lumbre.Interfaces.Repository;
using Lumbre.Interfaces.Contracts;
using System.Linq.Expressions;
using Hl7.Fhir.Model;
using Lumbre.Interfaces.Query;
using Lumbre.Interfaces.Query.Descriptors;

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
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);

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

        private IMongoCollection<BsonDocument> GetCollection(Primitives.CollectionName collectionName)
        {
            var client = CreateClient();

            var collection = client
                                .GetDatabase(_configuration.DatabaseName)
                                .GetCollection<BsonDocument>(collectionName);
            return collection;
        }

        public async Task<IRepositoryResult> Upsert(Primitives.CollectionName collectionName, Primitives.ResourceId resourceId, Primitives.JsonPayload payload)
        {
            var collection = GetCollection(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq(MongoDBId, resourceId.Id);
            var doc = BsonDocument.Parse(payload);
            doc.Set("_id", resourceId.Id);
            var result = await collection.ReplaceOneAsync(filter, doc, new ReplaceOptions { IsUpsert = true,  });

            return new Completed(1);
        }

        internal MongoClient CreateClient()
        {
            return new MongoClient(_configuration.ConnectionString);
        }

        public async Task<IRepositoryResult> DeleteById(Primitives.CollectionName collectionName, Primitives.ResourceId resourceId)
        {
            var collection = GetCollection(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq(MongoDBId, resourceId.Id);
            var result = await collection.DeleteOneAsync(filter);

            return result.DeletedCount switch
            {
                0 => new NotFound(),
                >= 1 => new Completed(result.DeletedCount),
                _ => throw new InvalidOperationException()
            };
        }

        public Task<IEnumerable<IRepositoryResult>> Query(Primitives.CollectionName collectionName, ISearchExpression[] Predicate)
        {
            throw new NotImplementedException();
        }
    }
}
