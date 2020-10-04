using Npgsql;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Webshop.Infrastructure.Data
{
	public interface IDataConnection
	{
		Task Execute(Func<NpgsqlConnection, Task> query, CancellationToken cancellationToken);
		Task<T> Execute<T>(Func<NpgsqlConnection, Task<T>> query, CancellationToken cancellationToken);
	}
}