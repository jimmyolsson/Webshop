using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Webshop.Infrastructure.Security.Identity.Entities
{
	public class ApplicationIdentityRoleClaim<TKey>
		where TKey : IEquatable<TKey>
	{
		public virtual int Id { get; set; }
		public virtual TKey RoleId { get; set; }
		public virtual string ClaimType { get; set; }
		public virtual string ClaimValue { get; set; }

		public virtual Claim ToClaim()
		{
			return new Claim(ClaimType, ClaimValue);
		}
	}
}
