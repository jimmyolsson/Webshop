using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Webshop.Infrastructure.Data
{
	public interface IBaseRepository
	{
		void Execute(Func<NpgsqlConnection, Task> query, CancellationToken cancellationToken);
		Task<T> Execute<T>(Func<NpgsqlConnection, Task<T>> query, CancellationToken cancellationToken);
	}
}
