using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Infrastructure.Configuration;
using Webshop.Infrastructure.Helpers;

namespace Webshop.Infrastructure.Data
{
	public class BaseRepository : IBaseRepository
	{
		private readonly IOptions<DatabaseOptions> _options;
		private readonly ILogger _logger;

		public BaseRepository(IOptions<DatabaseOptions> options, ILogger logger)
		{
			Guard.ArgumentNotNullOrWhiteSpace(options.Value.ConnectionString);

			_options = options;
			_logger = logger;
		}

		public async void Execute(Func<NpgsqlConnection, Task> query, CancellationToken cancellationToken)
		{
			try
			{
				using (var conn = new NpgsqlConnection(_options.Value.ConnectionString))
				{
					await conn.OpenAsync(cancellationToken);
					await query.Invoke(conn);
				}
			}
			catch(Exception e)
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
			catch(Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}
	}
}
