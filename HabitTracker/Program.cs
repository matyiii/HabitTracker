using Microsoft.Data.Sqlite; //Microsoft.Data.Sqlite nuget is kell nem cska a core

string connectionString = @"Data Source=habit-Tracker.db";

CreateDatabase();

GetUserInput();

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
            Environment.Exit(0); // nem szükséges, break is elég
            break;
        case "1":
            Read();
            Console.WriteLine("Read was succesfull.");
            break;
        case "2":
            Insert();
            Console.WriteLine("Insert was succesfull.");
            break;
        case "3":
            Console.WriteLine("Choose an ID");
            int id = int.Parse(Console.ReadLine());
            Update(id);
            Console.WriteLine("Update was succesfull.");
            break;
        default:
            break;
    }
}

//CRUD
void Read()
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();
            tableCmd.CommandText = @"SELECT Id,Date,Quantity FROM myHabit";
            //tableCmd.CommandType = System.Data.CommandType.Text;
            SqliteDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                Console.WriteLine("CRUD - Read");
                while (reader.Read())
                {
                    Console.WriteLine($"Id:{reader[0]}, Date:{reader[1]},Quantity:{reader[2]}");
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
        }
    }
}

void Insert()
{
    Console.WriteLine("What date you want to insert?");
    string? insertDate = Console.ReadLine();
    Console.WriteLine("Quantity?");
    int insertQuantity = int.Parse(Console.ReadLine());
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();
            tableCmd.CommandText = @$"INSERT INTO myHabit (Date,Quantity) VALUES ('{ insertDate}',{insertQuantity})";
            tableCmd.ExecuteReader();
        }
    }
}

void Update(int id) //TODO check if id exists in the table
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();
            Console.Write("New Date: ");//TODO: show the previous value
            string newDate = Console.ReadLine();
            Console.Write("New Quantity: ");
            int newQuantity = int.Parse(Console.ReadLine());
            tableCmd.CommandText = @$"UPDATE myHabit SET Date='{newDate}',Quantity={newQuantity} WHERE Id={id}";
            SqliteDataReader reader = tableCmd.ExecuteReader();
            if (reader.RecordsAffected==0)
                Console.WriteLine($"Id: {id} not found!");
        }
    }
}