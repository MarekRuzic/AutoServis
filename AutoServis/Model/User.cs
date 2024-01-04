using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServis.Model
{
    internal class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int admin { get; set; }

        public User() { }

        public User(int iD, string name, string lastname, string email, string password, int admin)
        {
            ID = iD;
            Name = name;
            this.lastname = lastname;
            this.email = email;
            this.password = password;
            this.admin = admin;
        }

        public bool checkPassword(string password)
        {
            //TODO: tady zařídit encrypt příchozího hesla
            return password == this.password;
        }
    }
}
