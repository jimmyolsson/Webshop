﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Infrastructure.Security.Identity.Entities;

namespace Webshop.Api.Entities
{
	public class ApplicationRole : ApplicationIdentityRole<int>
	{
		public ApplicationRole() { }
	}
}