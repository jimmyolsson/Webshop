using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Infrastructure.Security.Identity.Entities
{
	public class ApplicationIdentityUserRole<TKey> 
		where TKey : IEquatable<TKey>
	{
		public virtual TKey UserId { get; set; }
		public virtual TKey RoleId { get; set; }
	}
}
