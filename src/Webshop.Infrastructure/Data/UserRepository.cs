using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Infrastructure.Security.Identity.Entities;

namespace Webshop.Infrastructure.Data
{
	public class UserRepository<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole> :
		IUserRepository<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole>
		where TUser : ApplicationIdentityUser<TKey, TUserClaim, TUserRole, TUserLogin>
		where TKey : IEquatable<TKey>
		where TUserRole : ApplicationIdentityUserRole<TKey>
		where TRoleClaim : ApplicationIdentityRoleClaim<TKey>
		where TUserClaim : ApplicationIdentityUserClaim<TKey>
		where TUserLogin : ApplicationIdentityUserLogin<TKey>
		where TRole : ApplicationIdentityRole<TKey, TUserRole, TRoleClaim>
	{
		private readonly IRoleRepository<TRole, TKey, TUserRole, TRoleClaim> _roleRepository;
		private readonly IDataConnection _dataConnection;

		public UserRepository(IRoleRepository<TRole, TKey, TUserRole, TRoleClaim> roleRepository,
			IDataConnection dataConnection)
		{
			_roleRepository = roleRepository;
			_dataConnection = dataConnection;
		}

		public Task<IEnumerable<TUser>> GetAllAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

		public async Task<bool> AddToRoleAsync(TKey id, string roleName, CancellationToken cancellationToken)
		{
			var role = await _roleRepository.GetByNameAsync(roleName, cancellationToken);
			if (role == null)
				return false;

			var result = await _dataConnection.Execute(async conn =>
			{
				return await conn.QueryFirstOrDefaultAsync<TUserRole>(
					"INSERT INTO IdentityUserRoles VALUES(@UserId, @RoleId)",
					new { UserId = id, RoleId = role.Id });
			}, cancellationToken);

			return result == null ? false : true;
		}

		public async Task<TUser> GetByEmailAsync(string email, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var userDictionary = new Dictionary<TKey, TUser>();
				var queryResult = await conn.QueryAsync(
					"SELECT IdentityUsers.*, IdentityUserRoles.* FROM IdentityUsers " +
					"LEFT JOIN IdentityUserRoles ON IdentityUserRoles.\"UserId\" = IdentityUsers.\"Id\" WHERE UPPER(\"Email\") = @Email",
					param: new { Email = email },
					map: UserRoleMapping(userDictionary),
					splitOn: "UserId");

				return userDictionary;

			}, cancellationToken);

			if (result.Count > 0)
				return result.FirstOrDefault().Value;

			return null;
		}

		public async Task<TUser> GetByIdAsync(TKey id, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var userDictionary = new Dictionary<TKey, TUser>();
				var queryResult = await conn.QueryAsync(
					"SELECT IdentityUsers.*, IdentityUserRoles.* FROM IdentityUsers LEFT JOIN IdentityUserRoles ON IdentityUserRoles.\"UserId\" = IdentityUsers.\"Id\" WHERE \"Id\" = @Id",
					param: new { Id = id },
					map: UserRoleMapping(userDictionary),
					splitOn: "UserId");

				return userDictionary;

			}, cancellationToken);

			if (result.Count > 0)
				return result.FirstOrDefault().Value;

			return null;
		}

		public async Task<TUser> GetByUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var userDictionary = new Dictionary<TKey, TUser>();
				var queryResult = await conn.QueryAsync(
					"SELECT IdentityUsers.*, IdentityUserRoles.* FROM IdentityUsers " +
					"LEFT JOIN IdentityUserRoles ON IdentityUserRoles.\"UserId\" = IdentityUsers.\"Id\" " +
					"INNER JOIN IdentityUserLogins ON IdentityUsers.\"Id\" = IdentityUserLogins.\"UserId\" " +
					"WHERE \"LoginProvider\" = @LoginProvider AND \"ProviderKey\" = @ProviderKey LIMIT 1",
					param: new { LoginProvider = loginProvider, ProviderKey = providerKey },
					map: UserRoleMapping(userDictionary),
					splitOn: "UserId");

				return userDictionary;

			}, cancellationToken);

			if (result.Count > 0)
				return result.FirstOrDefault().Value;

			return null;
		}

		public async Task<TUser> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var userDictionary = new Dictionary<TKey, TUser>();

				var query = "SELECT IdentityUsers.*, IdentityUserRoles.* FROM IdentityUsers " +
					"LEFT JOIN IdentityUserRoles ON IdentityUserRoles.userid = IdentityUsers.id " +
					"WHERE UPPER(username) = @Username";

				var queryResult = await conn.QueryAsync(
					query,
					param: new { Username = userName.ToUpper() },
					map: UserRoleMapping(userDictionary),
					splitOn: "userid");

				return userDictionary;

			}, cancellationToken);

			if (result.Count > 0)
				return result.FirstOrDefault().Value;

			return null;
		}

		public async Task<IList<Claim>> GetClaimsByUserIdAsync(TKey id, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultQuery = await conn.QueryAsync(
					"SELECT \"ClaimType\", \"ClaimValue\" FROM IdentityUserClaims WHERE \"UserId\" = @Id",
					param: new { Id = id });

				return resultQuery?.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList();
			}, cancellationToken);

			return result;
		}

		public async Task<IList<string>> GetRolesByUserIdAsync(TKey id, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultQuery = await conn.QueryAsync<string>(
					"SELECT \"Name\" FROM IdentityRoles, IdentityUserRoles " +
					"WHERE IdentityRoles.\"Id\" = IdentityUserRoles.\"RoleId\" AND \"UserId\" = @Id",
					param: new { Id = id });

				return resultQuery.ToList();
			}, cancellationToken);

			return result;
		}

		public async Task<IList<UserLoginInfo>> GetUserLoginInfoByIdAsync(TKey id, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultQuery = await conn.QueryAsync(
					"SELECT \"LoginProvider\", \"Name\", \"ProviderKey\" FROM IdentityUserLogins " +
					"WHERE \"UserId\" = @Id",
					param: new { Id = id });

				return resultQuery?.Select(y => new UserLoginInfo(y.LoginProvider, y.ProviderKey, y.Name))
							.ToList();
			}, cancellationToken);

			return result;
		}

		public async Task<IList<TUser>> GetUsersByClaimAsync(Claim claim, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultQuery = await conn.QueryAsync<TUser>(
					"SELECT IdentityUsers.* FROM IdentityUsers, IdentityUserClaims " +
					"WHERE \"ClaimValue\" = @ClaimValue AND \"ClaimType\" = @ClaimType",
					param: new { ClaimValue = claim.Value, ClaimType = claim.Type });

				return resultQuery.ToList();
			}, cancellationToken);

			return result;
		}

		public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultQuery = await conn.QueryAsync<TUser>(
					"SELECT IdentityUsers.* FROM IdentityUsers, IdentityUserRoles, IdentityRoles " +
					"WHERE UPPER(IdentityRoles.\"Name\") = @RoleName AND IdentityUserRoles.\"RoleId\" = IdentityRoles.\"Id\"" +
					" AND IdentityUserRoles.\"UserId\" = IdentityUsers.\"Id\"",
					param: new { RoleName = roleName });

				return resultQuery.ToList();
			}, cancellationToken);

			return result;
		}

		public async Task<TKey> InsertAsync(TUser user, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultQuery = await conn.ExecuteScalarAsync<TKey>(
					"INSERT INTO IdentityUsers " +
					"(Id, Username, Email, EmailConfirmed, SecurityStamp, Password, LockoutEnabled, LockoutEnd, AccessFailedCount)" +
					"VALUES(@Id, @Username, @Email, @EmailConfirmed, @SecurityStamp, @Password, @LockoutEnabled, @LockoutEnd, @AccessFailedCount) " +
					" RETURNING Id",
					param: new
					{
						Id = user.Id,
						Username = user.Username,
						Email = user.Email,
						EmailConfirmed = user.EmailConfirmed,
						SecurityStamp = user.SecurityStamp,
						Password = user.Password,
						LockoutEnabled = user.LockoutEnabled,
						LockoutEnd = user.LockoutEnd,
						AccessFailedCount = user.AccessFailedCount
					});

				return resultQuery;
			}, cancellationToken);

			return result;
		}

		public async Task<bool> InsertClaimsAsync(TKey id, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultList = new List<bool>(claims.Count());
				foreach (var claim in claims)
				{
					var resultQuery = await conn.ExecuteAsync(
						"INSERT INTO IdentityUserClaim (\"ClaimType\", \"ClaimType\", \"UserId\") " +
						"VALUES (@ClaimType, @ClaimValue, @UserId)",
						param: new { ClaimType = claim.Type, ClaimValue = claim.Value, UserId = id });

					resultList.Add(resultQuery > 0);
				}

				return resultList.TrueForAll(x => x);
			}, cancellationToken);

			return result;
		}

		public async Task<bool> InsertLoginInfoAsync(TKey id, UserLoginInfo loginInfo, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultQuery = await conn.ExecuteAsync(
				"INSERT INTO \"dbo\".\"IdentityLogin\" (\"LoginProvider\", \"ProviderDisplayName\", \"ProviderKey\", \"UserId\") VALUES(@LoginProvider, @ProviderDisplayName, @ProviderKey, @UserId)",
				param: new
				{
					UserId = id,
					LoginProvider = loginInfo.LoginProvider,
					ProviderKey = loginInfo.ProviderKey,
					ProviderDisplay = loginInfo.ProviderDisplayName
				});

				return resultQuery > 0;

			}, cancellationToken);

			return result;
		}

		public async Task<bool> IsInRoleAsync(TKey id, string roleName, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultQuery = await conn.QueryAsync(
				"SELECT 1 FROM \"IdentityUser\", \"IdentityUserRole\", \"IdentityRole\" " +
				"WHERE UPPER(\"IdentityRole\".\"Name\") = @RoleName " +
				"AND \"IdentityUser\".\"Id\" = @UserId " +
				"AND \"IdentityUserRole\".\"RoleId\" = \"IdentityRole\".\"Id\" " +
				"AND \"IdentityUserRole\".\"UserId\" = \"IdentityUser\".\"Id\"",
				param: new
				{
					UserId = id,
					RoleName = roleName
				});

				return resultQuery.Count() > 0;

			}, cancellationToken);

			return result;
		}

		public async Task<bool> RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultQuery = await conn.ExecuteAsync(
				"DELETE FROM \"IdentityUser\" WHERE \"Id\" = @Id",
				param: new
				{
					Id = id,
				});

				return resultQuery > 0;

			}, cancellationToken);

			return result;
		}

		public async Task<bool> RemoveClaimsAsync(TKey id, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				var resultList = new List<bool>(claims.Count());
				foreach (var claim in claims)
				{
					var resultQuery = await conn.ExecuteAsync(
					"DELETE FROM \"IdentityUserClaim\" WHERE \"UserId\" = @UserId " +
					"AND \"ClaimType\" = @ClaimType " +
					"AND \"ClaimValue\" = @ClaimValue",
					param: new
					{
						UserId = id,
						ClaimValue = claim.Value,
						ClaimType = claim.Type
					});

					resultList.Add(resultQuery > 0);
				}

				return resultList.TrueForAll(x => x);

			}, cancellationToken);

			return result;
		}

		public async Task<bool> RemoveFromRoleAsync(TKey id, string roleName, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				return await conn.ExecuteAsync(
				"DELETE FROM \"IdentityUserRole\" USING \"IdentityRole\" " +
				"WHERE \"IdentityUserRole\".\"RoleId\" = \"IdentityRole\".\"Id\" " +
				"AND \"IdentityUserRole\".\"UserId\" = @UserId AND UPPER(\"IdentityRole\".\"Name\") = @RoleName",
				param: new
				{
					UserId = id,
					RoleName = roleName
				});

			}, cancellationToken);

			return result > 0;
		}

		public async Task<bool> RemoveLoginAsync(TKey id, string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				return await conn.ExecuteAsync(
				"DELETE FROM \"IdentityLogin\" " +
				"WHERE \"UserId\" = @UserId " +
				"AND \"LoginProvider\" = @LoginProvider " +
				"AND \"ProviderKey\" = @ProviderKey",
				param: new
				{
					UserId = id,
					LoginProvider = loginProvider,
					ProviderKey = providerKey
				});

			}, cancellationToken);

			return result > 0;
		}

		public async Task<bool> UpdateAsync(TUser user, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				return await conn.ExecuteAsync(
				"UPDATE \"IdentityUser\" SET " +
				"\"AccessFailedCount\" = @AccessFailedCount, " +
				"\"Email\" = @Email, " +
				"\"UserName\" = @UserName " +
				"\"EmailConfirmed\" = @EmailConfirmed, " +
				"\"LockoutEnabled\" = @LockoutEnabled, " +
				"\"LockoutEnd\" = @LockoutEnd, " +
				"\"PasswordHash\" = @PasswordHash, " +
				"\"SecurityStamp\" = @SecurityStamp, " +
				"\"TwoFactorEnabled\" = @TwoFactorEnabled, " +
				"WHERE \"Id\" = @Id",
				param: new
				{
					Id = user.Id,
					Username = user.Username,
					Email = user.Email,
					EmailConfirmed = user.EmailConfirmed,
					SecurityStamp = user.SecurityStamp,
					Password = user.Password,
					LockoutEnabled = user.LockoutEnabled,
					LockoutEnd = user.LockoutEnd,
					AccessFailedCount = user.AccessFailedCount
				});

			}, cancellationToken);

			return result > 0;
		}

		public async Task<bool> UpdateClaimAsync(TKey id, Claim oldClaim, Claim newClaim, CancellationToken cancellationToken)
		{
			var result = await _dataConnection.Execute(async conn =>
			{
				return await conn.ExecuteAsync(
				"UPDATE \"IdentityUserClaim\" SET " +
				"\"ClaimType\" = @NewClaimType, " +
				"\"ClaimValue\" = @NewClaimValue " +
				"WHERE " +
				"\"UserId\" = @UserId AND " +
				"\"ClaimType\" = @ClaimType AND " +
				"\"ClaimValue\" = @ClaimValue",
				param: new
				{
					UserId = id,
					NewClaimType = newClaim.Type,
					NewClaimValue = newClaim.Value,
					ClaimType = oldClaim.Type,
					ClaimValue = oldClaim.Value,
				});

			}, cancellationToken);

			return result > 0;
		}

		private static Func<TUser, TUserRole, TUser> UserRoleMapping(Dictionary<TKey, TUser> userDictionary)
		{
			return new Func<TUser, TUserRole, TUser>((user, role) =>
			{
				var dictionaryUser = default(TUser);

				if (role != null)
				{
					if (userDictionary.TryGetValue(user.Id, out dictionaryUser))
					{
						dictionaryUser.Roles.Add(role);
					}
					else
					{
						user.Roles.Add(role);
						userDictionary.Add(user.Id, user);

						dictionaryUser = user;
					}
				}
				else
				{
					dictionaryUser = user;
				}

				return dictionaryUser;
			});
		}
	}
}
