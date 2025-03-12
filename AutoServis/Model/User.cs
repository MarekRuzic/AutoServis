using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace AutoServis.Model
{
    public class User
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public User() { }

        public User(int iD, string firstname, string lastname, string email, string password)
        {
            id = iD;
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.password = password;
        }

        public User(string firstname, string lastname, string email, string password)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.password = password;
        }

        public User(int id, string firstname, string lastname, string email)
        {
            this.id = id;
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
        }

        public User(int id, string password)
        {
            this.id = id;
            this.password = password;
        }

        /// <summary>
        /// Používá Bcrypt.Verify, pro kontrolu zda nové heslo uživatele je stejné jako předchozí heslo
        /// </summary>
        /// <param name="password">Heslo ke kontrole</param>
        /// <returns>Pokud jsou stejná vrací true</returns>
        public bool checkPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, this.password);
        }

        public void UpdateUserCredits(string firstname, string lastname, string email)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
        }
    }
}
