using MatchServer;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System.Net.Sockets;
using System.Runtime.InteropServices;

class Program
{
    public static DB_Connector connector = new DB_Connector();

    static void Main(string[] args)
    {
        connector.connectDB();
        TCPServer server = new TCPServer();
        //connector.selectDB();
    }
}

class DB_Connector
{
    string _server = "localhost";
    int _port = 3306;
    string _database = "awsgameserver";
    string _id = "test";
    string _pw = "0000";
    string _connectionAddress = "";

    MySqlConnection conn = null;

    public DB_Connector()
    {
        _connectionAddress = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", _server, _port, _database, _id, _pw);
    }

    public void connectDB()
    {
        try
        {
            conn = new MySqlConnection(_connectionAddress);
            conn.Open();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void selectDB()
    {
        if (conn == null)
        {
            Console.WriteLine("데이터베이스에 연결되어있지 않습니다");
            return;
        }

        string query = string.Format("SELECT * FROM PlayerInfo");

        MySqlCommand command = new MySqlCommand(query, conn);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            PlayerInfo player = new PlayerInfo(int.Parse(reader["id"].ToString()), reader["user_id"].ToString(), reader["pwd"].ToString());
            Console.WriteLine(player.ToString());
        }
    }
}