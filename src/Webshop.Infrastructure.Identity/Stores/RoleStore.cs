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
	public class RoleStore<TRole, TKey, TUserRole, TRoleClaim> :
		IRoleStore<TRole>,
		IRoleClaimStore<TRole>
		where TRole : ApplicationIdentityRole<TKey, TUserRole, TRoleClaim>
		where TKey : IEquatable<TKey>
		where TUserRole : ApplicationIdentityUserRole<TKey>
		where TRoleClaim : ApplicationIdentityRoleClaim<TKey>
	{
		private readonly ILogger<RoleStore<TRole, TKey, TUserRole, TRoleClaim>> _logger;
		private readonly IRoleRepository<TRole, TKey, TUserRole, TRoleClaim> _roleRepository;

		public RoleStore(ILogger<RoleStore<TRole, TKey, TUserRole, TRoleClaim>> logger,
							IRoleRepository<TRole, TKey, TUserRole, TRoleClaim> roleRepo)
		{
			_roleRepository = roleRepo;
			_logger = logger;
		}

		public void Dispose()
		{
		}

		public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			try
			{
				var result = await _roleRepository.InsertAsync(role, cancellationToken);

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

		public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			try
			{
				var result = await _roleRepository.RemoveAsync(role.Id, cancellationToken);

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

		public virtual TKey ConvertIdFromString(string id)
		{
			if (id == null)
				return default(TKey);

			return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
		}

		public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNullOrWhiteSpace(roleId);

			try
			{
				var result = await _roleRepository.GetByIdAsync(ConvertIdFromString(roleId));

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNullOrWhiteSpace(normalizedRoleName);

			try
			{
				var result = await _roleRepository.GetByNameAsync(normalizedRoleName);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			return Task.FromResult(role.Name);
		}

		public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			if (role.Id.Equals(default(TKey)))
				return null;

			return Task.FromResult(role.Id.ToString());
		}

		public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			return Task.FromResult(role.Name);
		}

		public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			role.Name = normalizedName;

			return Task.CompletedTask;
		}

		public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			role.Name = roleName;

			return Task.CompletedTask;
		}

		public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			try
			{
				var result = await _roleRepository.UpdateAsync(role, cancellationToken);

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

		public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			try
			{
				var result = await _roleRepository.GetClaimsByRole(role, cancellationToken);

				return result?.Select(roleClaim => new Claim(roleClaim.ClaimType, roleClaim.ClaimValue))
							  .ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return null;
			}
		}

		public async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			try
			{
				var result = await _roleRepository.InsertClaimAsync(role, claim, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
		}

		public async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			Guard.ArgumentNotNull(role);

			try
			{
				var result = await _roleRepository.RemoveClaimAsync(role, claim, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
		}
	}
}
