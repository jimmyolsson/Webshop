using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Webshop.Infrastructure.Identity.Entities
{
	public class ApplicationIdentityUserClaim<TKey>
		where TKey : IEquatable<TKey>
	{
		public virtual int Id { get; set; }
		public virtual TKey UserId { get; set; }
		public virtual string ClaimType { get; set; }
		public virtual string ClaimValue { get; set; }

		public virtual Claim ToClaim() => new Claim(ClaimType, ClaimValue);
	}
}
