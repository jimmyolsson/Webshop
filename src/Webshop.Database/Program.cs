using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using System;
using System.Reflection;

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
			bool includeDefaultDevData = false;
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
						Console.WriteLine(connectionString);
						break;
					case string s when s.Contains("--defaultdata"):
						includeDefaultDevData = true;
						break;
					default:
						Console.WriteLine("Unknown argument");
						return -1;
				}
			}

			var upgraderBuilder = DeployChanges.To
									.PostgresqlDatabase(connectionString)
									.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script =>
									{
										return !script.ToLower().Contains("devdefault");
									})
									.LogToConsole();
			
			if (forceRunAllScripts)
			{
				upgraderBuilder.JournalTo(new NullJournal());
			}

			var result = upgraderBuilder.Build()
										.PerformUpgrade();

			if (result.Successful && includeDefaultDevData)
			{
				result = insertDefaultDevData(connectionString);
			}

			if (!result.Successful)
			{
				return printfailure(result.Error);
			}

			return printSuccess();
		}

		private static DatabaseUpgradeResult insertDefaultDevData(string connectionString)
		{
			var upgrader = DeployChanges.To
								.PostgresqlDatabase(connectionString)
								.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script =>
								{
									return script.ToLower().Contains("devdefault");
								})
								.LogToConsole()
								.Build();

			return upgrader.PerformUpgrade();
		}

		private static int printSuccess()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Success!");
			Console.ResetColor();
			return 0;
		}

		private static int printfailure(Exception exception)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(exception);
			Console.ResetColor();
#if DEBUG
			Console.ReadLine();
#endif
			return -1;
		}
	}
}
