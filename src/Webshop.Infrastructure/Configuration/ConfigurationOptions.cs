using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Infrastructure.Configuration
{
	public class ConfigurationOptions
	{
		public DatabaseOptions Database { get; set; }

		public ConfigurationOptions()
		{
			Database = new DatabaseOptions();
		}
	}
}
