﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Infrastructure.Security.Identity.Entities;

namespace Webshop.Infrastructure.Data
{
	public interface IUserRepository<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole>
		where TUser : ApplicationIdentityUser<TKey, TUserClaim, TUserRole, TUserLogin>
		where TKey : IEquatable<TKey>
		where TUserRole : ApplicationIdentityUserRole<TKey>
		where TRoleClaim : ApplicationIdentityRoleClaim<TKey>
		where TUserClaim : ApplicationIdentityUserClaim<TKey>
		where TUserLogin : ApplicationIdentityUserLogin<TKey>
		where TRole : ApplicationIdentityRole<TKey, TUserRole, TRoleClaim>
	{
		Task<TKey> InsertAsync(TUser user, CancellationToken cancellationToken);
		Task<bool> RemoveAsync(TKey id, CancellationToken cancellationToken);
		Task<bool> UpdateAsync(TUser user, CancellationToken cancellationToken);
		Task<TUser> GetByIdAsync(TKey id);
		Task<TUser> GetByUserNameAsync(string userName);
		Task<TUser> GetByEmailAsync(string email);
		Task<IEnumerable<TUser>> GetAllAsync();
		Task<TUser> GetByUserLoginAsync(string loginProvider, string providerKey);

		Task<bool> InsertClaimsAsync(TKey id, IEnumerable<Claim> claims, CancellationToken cancellationToken);
		Task<bool> InsertLoginInfoAsync(TKey id, UserLoginInfo loginInfo, CancellationToken cancellationToken);
		Task<bool> AddToRoleAsync(TKey id, string roleName, CancellationToken cancellationToken);

		Task<IList<Claim>> GetClaimsByUserIdAsync(TKey id);
		Task<IList<string>> GetRolesByUserIdAsync(TKey id);
		Task<IList<Microsoft.AspNetCore.Identity.UserLoginInfo>> GetUserLoginInfoByIdAsync(TKey id);
		Task<IList<TUser>> GetUsersByClaimAsync(Claim claim);
		Task<IList<TUser>> GetUsersInRoleAsync(string roleName);
		Task<bool> IsInRoleAsync(TKey id, string roleName);

		Task<bool> RemoveClaimsAsync(TKey id, IEnumerable<Claim> claims, CancellationToken cancellationToken);
		Task<bool> RemoveFromRoleAsync(TKey id, string roleName, CancellationToken cancellationToken);
		Task<bool> RemoveLoginAsync(TKey id, string loginProvider, string providerKey, CancellationToken cancellationToken);
		Task<bool> UpdateClaimAsync(TKey id, Claim oldClaim, Claim newClaim, CancellationToken cancellationToken);
	}
}
