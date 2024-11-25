using System.Data.SQLite;


namespace ChatHuone;

public class DatabaseHandler{

    private string _connectionString = "DataSource=chathuone.db";

    public void CreateDatabase(){
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        string query = "CREATE TABLE IF NOT EXISTS viestit (viesti_id INTEGER PRIMARY KEY, server_viesti_id INTEGER, lahettaja TEXT, viesti TEXT, timestamp DATETIME)";
        LuoPoyta(query, connection);
        connection.Close();
    }

    public void LuoPoyta(string query, SQLiteConnection connection){
        using var command = new SQLiteCommand(query, connection);
        command.ExecuteNonQuery();
    }

    public void LisaaViesti(Dictionary<string, string> viesti, int id){
        // string sqlFormattedDate = viesti.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
        string query = $"INSERT INTO viestit (server_viesti_id, lahettaja, viesti, timestamp) VALUES(\'{id}\', \'{viesti["Nimi"]}\', \'{viesti["Teksti"]}\', \'{viesti["TimeStamp"]}\')";

        try{
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = new SQLiteCommand(query, connection);
            var rowInserted = command.ExecuteNonQuery();
            connection.Close();
        }
        catch(SQLiteException ex){
            Console.WriteLine(ex.Message);
        }

    }

    public int ViimeisinId(){
        int id = 0;
        string hakuQuery = "SELECT server_viesti_id FROM viestit ORDER BY viesti_id DESC LIMIT 1";

        try{
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command = new SQLiteCommand(hakuQuery, connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }
        }
        catch(SQLiteException ex)
        {
        System.Console.WriteLine(ex.Message);
        }


        return id;
    }
}