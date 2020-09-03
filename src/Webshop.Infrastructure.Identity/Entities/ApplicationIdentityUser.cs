using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Infrastructure.Identity.Entities
{
	public class ApplicationIdentityUser<TKey, TUserClaim, TUserRole, TUserLogin>
		where TKey : IEquatable<TKey>
	{
		public virtual TKey Id { get; set; }
		public virtual string Username { get; set; }
		public virtual string Email { get; set; }
		public virtual bool EmailConfirmed { get; set; }
		public virtual string Password { get; set; }
		public virtual int AccessFailedCount { get; set; }
		public virtual string SecurityStamp { get; set; } = Guid.NewGuid().ToString();
		public virtual DateTimeOffset? LockoutEnd { get; set; }
		public virtual bool LockoutEnabled { get; set; }
		public virtual ICollection<TUserRole> Roles { get; set; } = new List<TUserRole>();
		public virtual ICollection<TUserClaim> Claims { get; set; } = new List<TUserClaim>();
		public virtual ICollection<TUserLogin> Logins { get; set; } = new List<TUserLogin>();

		public ApplicationIdentityUser() { }

		public ApplicationIdentityUser(string username) : this()
		{
			Username = username;
		}

		public override string ToString()
		{
			return Username;
		}
	}
}
