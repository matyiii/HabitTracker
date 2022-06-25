using Microsoft.Data.Sqlite; //Microsoft.Data.Sqlite NuGet is neccesery, Sqlite.Core is not enough

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
            tableCmd.ExecuteNonQuery(); //Execute without returing values
        }
        //using will close the connection no need to connection.Close()
    }
}

void GetUserInput()
{
    Console.WriteLine("\nWhat would you like to do?");

    Console.WriteLine("Type 0 to Close the App.");
    Console.WriteLine("Type 1 to View All Records.");
    Console.WriteLine("Type 2 to Insert Record.");
    Console.WriteLine("Type 3 to Update Record.");
    Console.WriteLine("Type 4 to Delete Record.");

    switch (Console.ReadLine())
    {
        case "0":
            Environment.Exit(0); // Not necessary, break does the job
            break;
        case "1":
            Read();
            Console.WriteLine("Read was succesfull.");
            GetUserInput();
            break;
        case "2":
            Insert();
            Console.WriteLine("Insert was succesfull.");
            GetUserInput();
            break;
        case "3":
            Console.WriteLine("Choose an ID");
            int idToUpdate = int.Parse(Console.ReadLine());
            if(ContainsId(idToUpdate))
            { 
                Console.WriteLine($"The Database contains this Id({idToUpdate})");//DEBUG purpose to test ContainsId
                Update(idToUpdate);
                Console.WriteLine("Update was succesfull.");
                GetUserInput();
            }
            else
            {
                Console.WriteLine($"The Database does not contains this Id({idToUpdate})");
                GetUserInput();
            }
            break;
        case "4":
            Console.WriteLine("Choose an ID to delete");
            int idToDelete = int.Parse(Console.ReadLine());
            if (ContainsId(idToDelete))
            {
                Delete(idToDelete);
                Console.WriteLine("Delete was succesfull.");
                GetUserInput();
            }
            else
            {
                Console.WriteLine($"The Database does not contains this Id({idToDelete})");
                GetUserInput();
            }
            break;
        default:
            Console.WriteLine("Wrong INPUT!");
            GetUserInput();
            break;
    }
}

//CRUD Operations
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

void Update(int id) //TODO check if id exists in the table -- ContainsId function
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();
            tableCmd.CommandText = $"SELECT * FROM myHabit WHERE Id={id}";
            SqliteDataReader reader2 = tableCmd.ExecuteReader();
            reader2.Read();
            Console.Write($"Old Date was:{reader2[1]}, New Date: ");//TODO: show the previous value
            string newDate = Console.ReadLine();
            Console.Write($"Old Quantity was:{reader2[2]}, New Quantity: ");
            reader2.Close();
            int newQuantity = int.Parse(Console.ReadLine());
            tableCmd.CommandText = @$"UPDATE myHabit SET Date='{newDate}',Quantity={newQuantity} WHERE Id={id}";
            SqliteDataReader reader1 = tableCmd.ExecuteReader();
            if (reader1.RecordsAffected==0)
                Console.WriteLine($"Id: {id} not found!");
        }
    }
}

void Delete(int id)
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();
            tableCmd.CommandText = $"DELETE FROM myHabit WHERE Id={id}";
            tableCmd.ExecuteNonQuery();
        }
    }
}

bool ContainsId(int id)
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();
            tableCmd.CommandText = $"SELECT Id FROM myHabit WHERE Id IN (SELECT Id FROM myHabit WHERE Id={id});"; //SQLite does not support ANY operator
            SqliteDataReader reader = tableCmd.ExecuteReader();
            return reader.HasRows ? true : false;
        }
    }
}