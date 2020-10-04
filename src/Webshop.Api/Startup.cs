using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Webshop.Api.Entities;
using Webshop.Infrastructure;
using Webshop.Infrastructure.Configuration;
using Webshop.Infrastructure.Security;

namespace Webshop.Api
{
	public class Startup
	{
		private IConfiguration _configuration { get; }

		public Startup(IWebHostEnvironment currentEnvironment, IConfiguration configuration)
		{
			_configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions<ConfigurationOptions>().Bind(_configuration)
				.ValidateDataAnnotations();

			services.ConfigureDapper();

			services.AddIdentity<ApplicationUser, ApplicationRole>(x =>
			{
				x.Password.RequiredLength = 8;
			}).AddDapperIdentityStores<int>().AddDefaultTokenProviders();

			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}
			app.UseAuthentication();

			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapDefaultControllerRoute();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}
