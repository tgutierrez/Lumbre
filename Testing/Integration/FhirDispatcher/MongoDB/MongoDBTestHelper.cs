using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Hl7.Fhir.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Hl7.Fhir.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Testing.Integration.FhirDispatcher.MongoDB
{
    internal static class MongoDBTestHelper 
    {
        public const string DbName = "Lumbre_Test";

        internal static void Initialize(string connectionString)
        {
            var client = new MongoClient(connectionString);

            try { client.DropDatabase(DbName); }
            catch { }

            // add test data
            var collection = client.GetDatabase(DbName).GetCollection<BsonDocument>("Patient");
            var jsonInput = File.ReadAllText("BaseTestData\\Patient.json");
            var doc = BsonDocument.Parse(jsonInput);
            collection.InsertOne(doc);
        }

        internal static void ExistOnDb(this Assert _, string connectionString, string collName, Func<FilterDefinitionBuilder<BsonDocument>, FilterDefinition<BsonDocument>> func) {
            var client = new MongoClient(connectionString);
            var collection = client.GetDatabase(DbName).GetCollection<BsonDocument>(collName);
            var filter = func(Builders<BsonDocument>.Filter);
            var doc = collection.Find(filter).FirstOrDefault();
            Assert.IsNotNull(doc);
        }
    }
}
