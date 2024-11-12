using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchServer
{
    internal class PlayerInfo
    {
        int id { get; set; }
        string user_id { get; set; }
        string pwd { get; set; } 

        public PlayerInfo(int id, string user_id, string pwd) {
            this.id = id;
            this.user_id = user_id; 
            this.pwd = pwd;
        }

        public override string ToString()
        {
            return "PlayerInfo id : " + id + " user_id : " + user_id + " pwd : " + pwd;
        }
    }
}
