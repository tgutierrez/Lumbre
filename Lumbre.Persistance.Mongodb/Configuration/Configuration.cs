using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Persistance.Mongodb.Configuration
{
    public class MongoDBConfigurator
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        internal RespositoryConfiguration GetConfig()
        {
            return new RespositoryConfiguration(ConnectionString, DatabaseName);  
        }
    }


    internal record RespositoryConfiguration(string ConnectionString, string DatabaseName);
}
