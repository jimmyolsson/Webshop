using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Infrastructure.Configuration;

namespace Webshop.Infrastructure.Data
{
	public class DataConnection : IDataConnection
    {
		private readonly IOptions<DatabaseOptions> _options;
		private readonly ILogger _logger;

		public DataConnection(IOptions<DatabaseOptions> options, ILogger logger)
		{
			_options = options;
			_logger = logger;
		}

		public ILogger Logger { get; }

		public async Task Execute(Func<NpgsqlConnection, Task> query, CancellationToken cancellationToken)
		{
			try
			{
				using (var conn = new NpgsqlConnection(_options.Value.ConnectionString))
				{
					await conn.OpenAsync(cancellationToken);
					await query.Invoke(conn);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}

		public async Task<T> Execute<T>(Func<NpgsqlConnection, Task<T>> query, CancellationToken cancellationToken)
		{
			try
			{
				using (var conn = new NpgsqlConnection(_options.Value.ConnectionString))
				{
					await conn.OpenAsync(cancellationToken);
					return await query.Invoke(conn);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}
	}
}
