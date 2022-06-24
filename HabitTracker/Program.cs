using Microsoft.Data.Sqlite; //Microsoft.Data.Sqlite nuget is kell nem cska a core

string connectionString = @"Data Source=habit-Tracker.db";

CreateDatabase();

GetUserInput();
Console.ReadLine();

void CreateDatabase()
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS myHabit (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Date TEXT,
                                    Quantity INTEGER)";
            tableCmd.ExecuteNonQuery(); //végrehajtja a parancsot ami nem lekérdezés és nem ad vissza adatot
        }
        //a using zárja a kapcsolatot nem kell külön
    }
}

void GetUserInput()
{
    Console.WriteLine("What would you like to do?");

    Console.WriteLine("Type 0 to Close the App.");
    Console.WriteLine("Type 1 to View All Records.");
    Console.WriteLine("Type 2 to Insert Record.");
    Console.WriteLine("Type 3 to Update Record.");
    Console.WriteLine("Type 4 to Delete Record.");

    switch (Console.ReadLine())
    {
        case "0":
            Console.WriteLine("TESZT0");
            break;
        case "1":
            Console.WriteLine("TESZT1");
            break;
        default:
            break;
    }
}