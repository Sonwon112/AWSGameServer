using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.X509;
using Google.Protobuf;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace MatchServer
{
    internal class TCPServer
    {
        private TcpListener server;
        private bool started = false;
        

        public TCPServer()
        {
            server = new TcpListener(IPAddress.Any, 7777);
            server.Start();
            started = true;
            Console.WriteLine("Server is Starting, WaitClient...");
            Thread acceptThread = new Thread(() => Accept());
            acceptThread.Start();
        }

        public void Accept()
        {
            while (started)
            {
                TcpClient client = server.AcceptTcpClient();
                if (client == null) continue;
                Console.WriteLine("client connected");

                IPEndPoint clientEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                string clientIP = "";
                if (clientEndPoint != null) {
                    clientIP = clientEndPoint.Address.ToString();
                    int clientPort = clientEndPoint.Port;
                    Console.WriteLine($"클라이언트 접속: IP = {clientIP}, 포트 = {clientPort}");

                    NetworkStream stream = client.GetStream();
                    DTO dto = new DTO(-1,"success");
                    String dataToJson = JsonSerializer.Serialize(dto);
                    Console.WriteLine(dataToJson);
                    byte[] buffer = Encoding.UTF8.GetBytes(dataToJson);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}


class DTO
{
    public int id { get; set; }
    public string msg { get; set; }

    public DTO(int  id, string msg)
    {
        this.id = id;
        this.msg = msg;
    }
}