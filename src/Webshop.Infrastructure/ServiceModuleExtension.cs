using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Core;
using Webshop.Infrastructure.Data;

namespace Webshop.Infrastructure
{
	public static class ServiceModuleExtension
	{
		public static void RegisterInfrastructureServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
		}
	}
}
