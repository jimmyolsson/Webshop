using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Infrastructure.Configuration;
using Webshop.Infrastructure.Helpers;
using Webshop.Infrastructure.Security.Identity.Entities;

namespace Webshop.Infrastructure.Data
{
	public class RoleRepository<TRole, TKey, TUserRole, TRoleClaim>
		: BaseRepository, IRoleRepository<TRole, TKey, TUserRole, TRoleClaim>
		where TRole : ApplicationIdentityRole<TKey, TUserRole, TRoleClaim>
		where TKey : IEquatable<TKey>
		where TUserRole : ApplicationIdentityUserRole<TKey>
		where TRoleClaim : ApplicationIdentityRoleClaim<TKey>
	{
		public RoleRepository(IOptions<DatabaseOptions> options,
			ILogger<RoleRepository<TRole, TKey, TUserRole, TRoleClaim>> logger)
			: base(options, logger)
		{
		}

		public async Task<TRole> GetByIdAsync(TKey id, CancellationToken cancellationToken)
		{
			return await Execute(async conn =>
			{
				return await conn.QueryFirstOrDefaultAsync<TRole>(
					"SELECT * FROM IdentityRoles WHERE Id = @Id",
					new { Id = id });
			}, cancellationToken);
		}

		public async Task<TRole> GetByNameAsync(string roleName, CancellationToken cancellationToken)
		{
			return await Execute(async conn =>
			{
				return await conn.QueryFirstOrDefaultAsync<TRole>(
					"SELECT * FROM IdentityRoles WHERE UPPER(Name) = @Name",
					new { Name = roleName });
			}, cancellationToken);
		}

		public async Task<IEnumerable<TRoleClaim>> GetClaimsByRole(TRole role, CancellationToken cancellationToken)
		{
			return await Execute(async conn =>
			{
				return await conn.QueryAsync<TRoleClaim>(
					"SELECT * FROM IdentityRoles, IdentityRoleClaims WHERE RoleId = @RoleId AND IdentityRoleClaims.Id = @RoleId",
					new { RoleId = role.Id });
			}, cancellationToken);
		}

		public async Task<bool> InsertAsync(TRole role, CancellationToken cancellationToken)
		{
			var result = await Execute(async conn =>
			{
				return await conn.QueryAsync<TRole>(
					"INSERT INTO IdentityRoles VALUES(@RoleId, @RoleName)",
					new { RoleId = role.Id, RoleName = role.Name });
			}, cancellationToken);

			return result.Count() > 0;
		}

		public async Task<bool> InsertClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken)
		{
			var result = await Execute(async conn =>
			{
				return await conn.QueryAsync<TRoleClaim>(
					"INSERT INTO IdentityRoleClaims VALUES(@RoleId, @RoleName, @ClaimType, @ClaimValue)",
					new { RoleId = role.Id, RoleName = role.Name, ClaimType = claim.Type, ClaimValue = claim.Value });
			}, cancellationToken);

			return result.Count() > 0;
		}

		public async Task<bool> RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			var result = await Execute(async conn =>
			{
				return await conn.QueryAsync(
					"DELETE FROM IdentityRoleClaims WHERE \"Id\" = @Id",
					new { Id = id });
			}, cancellationToken);

			return result.Count() > 0;
		}

		public async Task<bool> RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken)
		{
			var result = await Execute(async conn =>
			{
				return await conn.QueryAsync(
					"DELETE FROM IdentityRoleClaims WHERE \"RoleId\" = @RoleId AND \"ClaimType\" = @ClaimType AND \"ClaimValue\" = @ClaimValue",
					new { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
			}, cancellationToken);

			return result.Count() > 0;
		}

		public async Task<bool> UpdateAsync(TRole role, CancellationToken cancellationToken)
		{
			var result = await Execute(async conn =>
			{
				return await conn.QueryAsync<TRole>(
					"UPDATE IdentityRoles SET \"Id\" = @RoleId, \"Name\" = @RoleName WHERE \"Id\" = %ID%",
					new { RoleId = role.Id, RoleName = role.Name });
			}, cancellationToken);

			return result.Count() > 0;
		}
	}
}
