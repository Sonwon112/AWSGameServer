using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MatchServer
{
    internal class Client
    {
        private NetworkStream stream;
        private int id;
        private string nickname;
        private bool isOpen = false;
        private DB_Connector connector;

        public Client(int id, NetworkStream stream)
        {   
            this.id = id;
            this.stream = stream;
            this.isOpen = true;
            connector = DB_Connector.getInstance();

            Thread thread = new Thread(() => Listen());
            thread.Start();
        }

        public void Listen()
        {
            Console.WriteLine("start Listen");
            byte[] buffer = new byte[1024];
            while (isOpen)
            {
                try
                {
                    int byteRead = stream.Read(buffer, 0, buffer.Length);
                    if (byteRead < 0) continue;
                    string receivedData = Encoding.UTF8.GetString(buffer);
                    receivedData = cleanJson(receivedData);
                    Console.WriteLine(receivedData);
                    DTO dto = JsonSerializer.Deserialize<DTO>(receivedData);

                    if (dto == null)
                    {
                        Console.WriteLine("dto가 존재하지 않습니다");
                        continue;
                    }

                    if (dto.id == id)
                    {
                        Type dtoType = (Type)Enum.Parse(typeof(Type), dto.type);
                        switch (dtoType)
                        {
                            case Type.LOGIN:
                                string[] data = dto.msg.Split(';');
                                UserInfo user = connector.selectUserInfoByUserId(data[0]);
                                if (user == null)
                                {
                                    Console.WriteLine("등록되지 않은 사용자 입니다.");
                                    Send(Type.LOGIN, "fail;01");
                                    continue;
                                }
                                if (!data[1].Equals(user.pwd))
                                {
                                    Console.WriteLine("잘못된 비밀번호 입니다.");
                                    Send(Type.LOGIN, "fail;02");
                                    continue;
                                }
                                Console.WriteLine(data[0] + "님 로그인 성공");
                                Send(Type.LOGIN, "success;" + user.nickname);
                                break;
                            case Type.SIGNUP:
                                break;
                            case Type.START:
                                break;
                            case Type.END:
                                break;
                        }
                    }
                }catch (Exception ex) { 
                    Console.WriteLine(ex.ToString()); 
                    isOpen = false;
                }
                
            }
        }

        public void Send(Type type, string msg)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    DTO dto = new DTO(-1, type.ToString(), msg);
                    string dtoToJson = JsonSerializer.Serialize(dto);
                    byte[] buffer = Encoding.UTF8.GetBytes(dtoToJson);
                    stream.Write(buffer, 0, buffer.Length);
                }catch (Exception ex) { 
                    Console.WriteLine(ex.Message);
                    isOpen = false;
                }
                
            });
            thread.Start();
        }

        public string cleanJson(string json)
        {
            int lastIndex = json.LastIndexOf("}");
            json = json.Substring(0, lastIndex + 1);
            return json;
        }
    }
}

public enum Type
{
    CONNECT,
    LOGIN,
    SIGNUP,
    START,
    END
}
