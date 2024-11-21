using MatchServer;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System.Net.Sockets;
using System.Runtime.InteropServices;

class Program
{

    static void Main(string[] args)
    {
        TCPServer server = new TCPServer();
        DB_Connector.getInstance().connectDB();
        //DB_Connector.getInstance().selectDB();
    }
}

class DB_Connector
{
    private static DB_Connector instance = new DB_Connector();

    public static DB_Connector getInstance(){return instance;}

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
            UserInfo player = new UserInfo(reader["user_id"].ToString(), reader["pwd"].ToString(), reader["nickname"].ToString());
            Console.WriteLine(player.ToString());
        }
        reader.Close();
    }

    public UserInfo selectUserInfoByUserId(string userId)
    {
        if(conn == null)
        {
            Console.WriteLine("데이터베이스에 연결되어있지 않습니다");
            return null;
        }

        string query = string.Format("SELECT user_id, pwd, nickname FROM PlayerInfo WHERE user_id=\"{0}\"", userId);
        MySqlCommand command = new MySqlCommand(query, conn);
        MySqlDataReader reader = command.ExecuteReader();
        UserInfo player = null;
        while (reader.Read())
        {
            player = new UserInfo(reader["user_id"].ToString(), reader["pwd"].ToString(), reader["nickname"].ToString());
        }
        reader.Close();

        return player; 
    }
}