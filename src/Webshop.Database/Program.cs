using DbUp;
using DbUp.Builder;
using DbUp.Engine;
using DbUp.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Webshop.Database
{
	class Program
	{
		/*
		 NOTE: 
		 This assumes that you've already created your database and user(s) with the correct permissions.
		 */
		static int Main(string[] args)
		{
			bool forceRunAllScripts = false;
			string connectionString = string.Empty;
			string scriptsPath = string.Empty;

			foreach (var arg in args)
			{
				switch (arg)
				{
					case "-f":
						forceRunAllScripts = true;
						break;
					case string s when s.Contains("--connectionstring"):
						connectionString = arg.Substring(arg.IndexOf('=') + 1).Replace("\"", string.Empty);
						break;
					default:
						Console.WriteLine("Unknown argument");
						return -1;
				}
			}

			var upgrader = forceRunAllScripts ? DeployChanges.To
					.PostgresqlDatabase(connectionString)
					.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
					.LogToConsole()
					.JournalTo(new NullJournal())
					.Build()
			:
			DeployChanges.To
					.PostgresqlDatabase(connectionString)
					.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
					.LogToConsole()
					.Build();

			var result = upgrader.PerformUpgrade();

			if (!result.Successful)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(result.Error);
				Console.ResetColor();
#if DEBUG
				Console.ReadLine();
#endif
				return -1;
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Success!");
			Console.ResetColor();
			return 0;
		}
	}
}
