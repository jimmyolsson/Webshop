using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Webshop.Infrastructure.Data;
using Webshop.Infrastructure.Security.Identity.Entities;
using Webshop.Infrastructure.Security.Identity.Stores;

namespace Webshop.Infrastructure
{
	public static class ServiceModuleExtension
	{
		public static IServiceCollection ConfigureDapper(this IServiceCollection services)
		{
			services.AddScoped<IDataConnection, DataConnection>();
			return services;
		}

		public static IdentityBuilder AddDapperIdentityStores<TKey>(this IdentityBuilder builder)
		{
			var userType = builder.UserType;
			var roleType = builder.RoleType;

			Type userStoreType;
			Type roleStoreType;

			var keyType = typeof(TKey);

			var userRoleType = typeof(ApplicationIdentityUserRole<>).MakeGenericType(keyType);
			var roleClaimType = typeof(ApplicationIdentityRoleClaim<>).MakeGenericType(keyType);
			var userClaimType = typeof(ApplicationIdentityUserClaim<>).MakeGenericType(keyType);
			var userLoginType = typeof(ApplicationIdentityUserLogin<>).MakeGenericType(keyType);

			userStoreType = typeof(IdentityUserStore<,,,,,,>).MakeGenericType(userType, keyType, userRoleType, roleClaimType,
																		   userClaimType, userLoginType, roleType);
			roleStoreType = typeof(IdentityRoleStore<,,,>).MakeGenericType(roleType, keyType, userRoleType, roleClaimType);

			builder.Services.AddScoped(typeof(IRoleRepository<,,,>).MakeGenericType(roleType, keyType, userRoleType, roleClaimType),
							   typeof(RoleRepository<,,,>).MakeGenericType(roleType, keyType, userRoleType, roleClaimType));

			builder.Services.AddScoped(typeof(IUserRepository<,,,,,,>).MakeGenericType(userType, keyType, userRoleType,
																			  roleClaimType, userClaimType,
																			  userLoginType, roleType),
							   typeof(UserRepository<,,,,,,>).MakeGenericType(userType, keyType, userRoleType,
																			 roleClaimType, userClaimType,
																			 userLoginType, roleType));

			builder.Services.AddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
			builder.Services.AddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);

			return builder;
		}
	}
}
