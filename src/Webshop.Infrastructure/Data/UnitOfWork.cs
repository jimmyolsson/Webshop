using System;
using Webshop.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Webshop.Infrastructure.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private IServiceProvider _serviceProvider;

		public UnitOfWork(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}
	}
}
