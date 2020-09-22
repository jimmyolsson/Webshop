using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Infrastructure.Security.Identity.Entities
{
	public class ApplicationIdentityRole<TKey> : ApplicationIdentityRole<TKey, ApplicationIdentityUserRole<TKey>, ApplicationIdentityRoleClaim<TKey>>
		where TKey : IEquatable<TKey>
	{
		public ApplicationIdentityRole() { }
	}

	public class ApplicationIdentityRole<TKey, TUserRole, TRoleClaim>
		where TKey : IEquatable<TKey>
		where TUserRole : ApplicationIdentityUserRole<TKey>
		where TRoleClaim : ApplicationIdentityRoleClaim<TKey>
	{
		public virtual TKey Id { get; set; }
		public virtual string Name { get; set; }
		public virtual ICollection<TUserRole> Users { get; set; } = new List<TUserRole>();
		public virtual ICollection<TRoleClaim> Claims { get; set; } = new List<TRoleClaim>();

		public ApplicationIdentityRole() { }
		public ApplicationIdentityRole(string roleName) : this()
		{
			Name = roleName;
		}
	}
}
