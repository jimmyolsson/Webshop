using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Infrastructure.Security.Identity.Entities
{
	public class ApplicationIdentityUserLogin<TKey>
		where TKey : IEquatable<TKey>
	{
		public virtual string LoginProvider { get; set; }
		public virtual string ProviderKey { get; set; }
		public virtual string ProviderDisplayName { get; set; }
		public virtual TKey UserId { get; set; }
	}
}
