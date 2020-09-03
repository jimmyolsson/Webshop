using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Infrastructure.Configuration;
using Webshop.Infrastructure.Security.Identity.Entities;

namespace Webshop.Infrastructure.Data
{
	internal class UserRepository<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole> :
		IUserRepository<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole>
		where TKey : IEquatable<TKey>
		where TUser : ApplicationIdentityUser<TKey, TUserClaim, TUserRole, TUserLogin>
		where TUserRole : ApplicationIdentityUserRole<TKey>
		where TRoleClaim : ApplicationIdentityRoleClaim<TKey>
		where TUserClaim : ApplicationIdentityUserClaim<TKey>
		where TUserLogin : ApplicationIdentityUserLogin<TKey>
		where TRole : ApplicationIdentityRole<TKey, TUserRole, TRoleClaim>
	{
		private readonly IDataConnection _dataConnection;

		internal UserRepository(IDataConnection dataConnection)
		{
			_dataConnection = dataConnection;
		}

		public Task<bool> AddToRoleAsync(TKey id, string roleName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TUser>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<TUser> GetByEmailAsync(string email)
		{
			throw new NotImplementedException();
		}

		public Task<TUser> GetByIdAsync(TKey id)
		{
			throw new NotImplementedException();
		}

		public Task<TUser> GetByUserLoginAsync(string loginProvider, string providerKey)
		{
			throw new NotImplementedException();
		}

		public Task<TUser> GetByUserNameAsync(string userName)
		{
			throw new NotImplementedException();
		}

		public Task<IList<Claim>> GetClaimsByUserIdAsync(TKey id)
		{
			throw new NotImplementedException();
		}

		public Task<IList<string>> GetRolesByUserIdAsync(TKey id)
		{
			throw new NotImplementedException();
		}

		public Task<IList<UserLoginInfo>> GetUserLoginInfoByIdAsync(TKey id)
		{
			throw new NotImplementedException();
		}

		public Task<IList<TUser>> GetUsersByClaimAsync(Claim claim)
		{
			throw new NotImplementedException();
		}

		public Task<IList<TUser>> GetUsersInRoleAsync(string roleName)
		{
			throw new NotImplementedException();
		}

		public Task<TKey> InsertAsync(TUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> InsertClaimsAsync(TKey id, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> InsertLoginInfoAsync(TKey id, UserLoginInfo loginInfo, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> IsInRoleAsync(TKey id, string roleName)
		{
			throw new NotImplementedException();
		}

		public Task<bool> RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> RemoveClaimsAsync(TKey id, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> RemoveFromRoleAsync(TKey id, string roleName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> RemoveLoginAsync(TKey id, string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateAsync(TUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateClaimAsync(TKey id, Claim oldClaim, Claim newClaim, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
