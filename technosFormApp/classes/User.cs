using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace technosFormApp.classes
{
    internal class User
    {
        public string? email;
        public string? password;
        public int? id;
        public User(string? nemail, string? npassword, int? nid)
        {
            email = nemail;
            password = npassword;
            id = nid;   
        }
    }
}
