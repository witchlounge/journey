using System.Reflection;

using DbUp;

namespace WitchLounge.Journey.Database;

public class Class1
{
    public void method1()
    {
        var connectionString = "Server=localhost; Username=postgres; Password=postgres";

        var upgrader = DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

        var result = upgrader.PerformUpgrade();
    }
}