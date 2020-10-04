using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Infrastructure.Configuration;
using Webshop.Infrastructure.Data;

namespace Webshop.IntegrationTests.TestDBConnection
{
    public static class TestDBConnectionProvider
    {
        private static readonly string _developmentConnectionString = "User ID=webshop;Password=webshop;Server=192.168.99.100;Port=5432;Database=webshopdb;";

        private static readonly string _integrationConnectionString = "User ID=webshopTester;Password=webshopTester;Server=192.168.99.100;Port=5432;Database=webshop-integration-test-db;";
    
        public static async Task<DataConnection> GetDataConnection()
        {
            IOptions<DatabaseOptions> options = Options.Create(new DatabaseOptions
            {
                ConnectionString = _developmentConnectionString
            });

            var logger = new Mock<ILogger>();

            var dataConnection = new DataConnection(options, logger.Object);
            await seedDataBase(dataConnection);

            return dataConnection;
        }

        private static async Task seedDataBase(DataConnection dataConnection, CancellationToken cancellationToken = new CancellationToken())
        {
            await dataConnection.Execute(async conn =>
            {
                await conn.ExecuteAsync("INSERT INTO IdentityUsers " +
                    "(Id, Username, Email, EmailConfirmed, SecurityStamp, Password, LockoutEnabled, LockoutEnd, AccessFailedCount)" +
                    "VALUES(1, 'Test', 'Test@test.com', true, 123, 'SuperSecret', false, NULL, 0)");
            }, cancellationToken);
        }
    }
}
