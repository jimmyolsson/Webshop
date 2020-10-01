using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Webshop.Infrastructure.Configuration;
using Webshop.Infrastructure.Data;

namespace Webshop.IntegrationTests.TestDBConnection
{
    public static class TestDBConnectionProvider
    {
        private static readonly string _connectionString = "User ID=webshopTester;Password=webshopTester;Server=192.168.99.100;Port=5432;Database=webshop-integration-test-db;";
    
        public static IDataConnection GetDataConnection()
        {
            IOptions<DatabaseOptions> options = Options.Create(new DatabaseOptions
            {
                ConnectionString = _connectionString
            });

            var logger = new Mock<ILogger>();

            return new DataConnection(options, logger.Object);
        }
    }
}
