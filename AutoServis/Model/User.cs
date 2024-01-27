using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServis.Model
{
    internal class User
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int admin { get; set; }

        public User() { }

        public User(int iD, string firstname, string lastname, string email, string password, int admin)
        {
            id = iD;
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.password = password;
            this.admin = admin;
        }

        public User(string firstname, string lastname, string email, string password)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.password = password;
        }

        public bool checkPassword(string password)
        {
            //TODO: tady zařídit encrypt příchozího hesla
            return password == this.password;
        }
    }
}
