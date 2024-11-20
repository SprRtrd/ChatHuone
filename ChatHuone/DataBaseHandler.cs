using System.Data.SQLite;


namespace ChatHuone;

public class DatabaseHandler{

    private string _connectionString = "DataSource=chathuone.db";

    public void CreateDatabase(){
        using (var connection = new SQLiteConnection(_connectionString)){
            connection.Open();
            string query = "CREATE TABLE IF NOT EXISTS viestit (viesti_id INTEGER PRIMARY KEY, lahettaja TEXT, viesti TEXT, timestamp DATETIME)";
            LuoPoyta(query, connection);
            connection.Close();
        }
    }

    public void LuoPoyta(string query, SQLiteConnection connection){
        using (var command = new SQLiteCommand(query, connection)){
            command.ExecuteNonQuery();
        }
    }

    public void LisaaViesti(Viesti viesti){
        string sqlFormattedDate = viesti.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
        string query = $"INSERT INTO viestit (lahettaja, viesti, timestamp) VALUES(\'{viesti.Nimi}\', \'{viesti.Teksti}\', \'{sqlFormattedDate}\')";

        try{
            using (var connection = new SQLiteConnection(_connectionString)){
                connection.Open();
                using var command = new SQLiteCommand(query, connection);
                var rowInserted = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch(SQLiteException ex){
            Console.WriteLine(ex.Message);
        }

    }
}