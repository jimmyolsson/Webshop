using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Webshop.Infrastructure.Configuration;

namespace Webshop.Infrastructure.Data
{
	public abstract class BaseRepository
	{
		private readonly DatabaseOptions _databaseOptions;

		public BaseRepository(IOptions<ConfigurationOptions> configuration)
		{
			_databaseOptions = configuration?.Value?.Database;
		}

		protected NpgsqlConnection GetConnection() 
		{
			return new NpgsqlConnection(_databaseOptions.ConnectionString);
		}
	}
}
