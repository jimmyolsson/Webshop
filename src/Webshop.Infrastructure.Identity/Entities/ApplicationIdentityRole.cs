using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Infrastructure.Security.Identity.Entities;

namespace Webshop.Infrastructure.Identity.Entities
{
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
