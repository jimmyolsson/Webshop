using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Infrastructure.Data;
using Webshop.Infrastructure.Helpers;
using Webshop.Infrastructure.Security.Identity.Entities;

namespace Webshop.Infrastructure.Security.Identity.Stores
{
	public class UserStore<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole> :
		IUserStore<TUser>,
		IUserLoginStore<TUser>,
		IUserRoleStore<TUser>,
		IUserClaimStore<TUser>,
		IUserPasswordStore<TUser>,
		IUserSecurityStampStore<TUser>,
		IUserEmailStore<TUser>,
		IUserLockoutStore<TUser>,
		IQueryableUserStore<TUser>,
		IUserAuthenticationTokenStore<TUser>
		where TUser : ApplicationIdentityUser<TKey, TUserClaim, TUserRole, TUserLogin>
		where TKey : IEquatable<TKey>
		where TUserRole : ApplicationIdentityUserRole<TKey>
		where TRoleClaim : ApplicationIdentityRoleClaim<TKey>
		where TUserClaim : ApplicationIdentityUserClaim<TKey>
		where TUserLogin : ApplicationIdentityUserLogin<TKey>
		where TRole : ApplicationIdentityRole<TKey, TUserRole, TRoleClaim>
	{
		// We're using dapper..
		public IQueryable<TUser> Users => throw new NotImplementedException();

		private readonly IUserRepository<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole> _userRepository;
		private readonly ILogger<UserStore<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole>> _logger;

		internal UserStore(IUserRepository<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole> userRepository,
			ILogger<UserStore<TUser, TKey, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TRole>> logger)
		{
			_userRepository = userRepository;
			_logger = logger;
		}

		public void Dispose()
		{
		}

		public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			try
			{
				var result = await _userRepository.InsertClaimsAsync(user.Id, claims, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
		}

		public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			try
			{
				var result = await _userRepository.InsertLoginInfoAsync(user.Id, login, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
		}

		public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			try
			{
				var result = await _userRepository.AddToRoleAsync(user.Id, roleName, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
		}

		public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			try
			{
				var result = await _userRepository.InsertAsync(user, cancellationToken);

				if (!result.Equals(default(TKey)))
				{
					user.Id = result;

					return IdentityResult.Success;
				}
				else
				{
					return IdentityResult.Failed();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return IdentityResult.Failed(new IdentityError[]
				{
					new IdentityError{ Description = ex.Message }
				});
			}
		}

		public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			try
			{
				var result = await _userRepository.RemoveAsync(user.Id, cancellationToken);

				return result ? IdentityResult.Success : IdentityResult.Failed();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return IdentityResult.Failed(new IdentityError[]
				{
					new IdentityError{ Description = ex.Message }
				});
			}
		}

		public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNullOrWhiteSpace(normalizedEmail);

			try
			{
				var result = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}

		}

		public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNullOrWhiteSpace(userId);

			try
			{
				var key = default(TKey);

				var converter = TypeDescriptor.GetConverter(typeof(TKey));
				if (converter != null && converter.CanConvertFrom(typeof(string)))
				{
					key = (TKey)converter.ConvertFromInvariantString(userId);
				}
				else
				{
					key = (TKey)Convert.ChangeType(userId, typeof(TKey));
				}

				var result = await _userRepository.GetByIdAsync(key, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNullOrWhiteSpace(loginProvider);
			Guard.ArgumentNotNullOrWhiteSpace(providerKey);

			try
			{
				var result = await _userRepository.GetByUserLoginAsync(loginProvider, providerKey, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNullOrWhiteSpace(normalizedUserName);

			try
			{
				var result = await _userRepository.GetByUserNameAsync(normalizedUserName, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.AccessFailedCount);
		}

		public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			try
			{
				var result = await _userRepository.GetClaimsByUserIdAsync(user.Id, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.EmailConfirmed);
		}

		public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.LockoutEnabled);
		}

		public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.LockoutEnd);
		}

		public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			try
			{
				var result = await _userRepository.GetUserLoginInfoByIdAsync(user.Id, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.Email);
		}

		public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.Username);
		}

		public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.Password);
		}

		public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			try
			{
				var result = await _userRepository.GetRolesByUserIdAsync(user.Id, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

#warning GetTokenAsync only returns string.Empty
		public Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
		{
			return Task.FromResult(string.Empty);
		}

		public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.Id.ToString());
		}

		public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.Username);
		}

		public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(claim);

			try
			{
				var result = await _userRepository.GetUsersByClaimAsync(claim, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNullOrWhiteSpace(roleName);

			try
			{
				var result = await _userRepository.GetUsersInRoleAsync(roleName, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.Password != null);
		}

		public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			user.AccessFailedCount++;
			return Task.FromResult(user.AccessFailedCount);
		}

		public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);
			Guard.ArgumentNotNullOrWhiteSpace(roleName);

			try
			{
				var result = await _userRepository.IsInRoleAsync(user.Id, roleName, cancellationToken);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return false;
			}
		}

		public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);
			Guard.ArgumentNotNull(claims);

			try
			{
				var result = await _userRepository.RemoveClaimsAsync(user.Id, claims, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
		}

		public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);
			Guard.ArgumentNotNullOrWhiteSpace(roleName);

			try
			{
				var result = await _userRepository.RemoveFromRoleAsync(user.Id, roleName, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
		}

		public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);
			Guard.ArgumentNotNullOrWhiteSpace(loginProvider);
			Guard.ArgumentNotNullOrWhiteSpace(providerKey);

			try
			{
				var result = await _userRepository.RemoveLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
		}

#warning RemoveTokenAsync not implemented!
		public Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);
			Guard.ArgumentNotNull(claim);
			Guard.ArgumentNotNull(newClaim);

			try
			{
				var result = await _userRepository.UpdateClaimAsync(user.Id, claim, newClaim, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
		}

		public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			user.AccessFailedCount = 0;

			return Task.CompletedTask;
		}

		public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			user.Email = email;

			return Task.CompletedTask;
		}

		public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			user.EmailConfirmed = confirmed;

			return Task.CompletedTask;
		}

		public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			user.LockoutEnabled = enabled;

			return Task.CompletedTask;
		}

		public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			user.LockoutEnd = lockoutEnd;

			return Task.CompletedTask;
		}

		public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);
			Guard.ArgumentNotNullOrWhiteSpace(normalizedEmail);

			user.Email = normalizedEmail;

			return Task.CompletedTask;
		}

		public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.CompletedTask;
		}

		public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);
			Guard.ArgumentNotNullOrWhiteSpace(passwordHash);

			user.Password = passwordHash;

			return Task.CompletedTask;
		}

		public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			return Task.FromResult(user.SecurityStamp);
		}

		public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);

			user.SecurityStamp = stamp;

			return Task.CompletedTask;
		}

		public Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public Task SetUserNameAsync(TUser user, string username, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(user);
			Guard.ArgumentNotNullOrWhiteSpace(username);

			user.Username = username;

			return Task.CompletedTask;
		}

		public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			try
			{
				var result = await _userRepository.UpdateAsync(user, cancellationToken);

				return result ? IdentityResult.Success : IdentityResult.Failed();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return IdentityResult.Failed(new IdentityError[]
				{
					new IdentityError{ Description = ex.Message }
				});
			}
		}
	}
}
