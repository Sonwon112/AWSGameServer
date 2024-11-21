using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchServer
{
    internal class UserInfo
    {
        public string user_id { get; set; }
        public string pwd { get; set; }
        public string nickname { get; set; }

        public UserInfo(string user_id, string pwd, string nickname)
        {
            this.user_id = user_id;
            this.pwd = pwd;
            this.nickname = nickname;
        }

        public override string ToString()
        {
            return "User : " + user_id + ", Pwd : " + pwd + ", Nickname : " + nickname;

        }
    }
}
