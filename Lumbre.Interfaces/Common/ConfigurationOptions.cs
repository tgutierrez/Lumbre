using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Common
{
    public class ConfigurationOptions
    {
        public string ConnectionString { get; set; }

        public Configuration GetConfiguration()
        {
            return new Configuration(ConnectionString);
        }

    }

    public record Configuration(string ConnectionString);
}
