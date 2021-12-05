using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwManager.Entities
{
    public class User
    {
        private int id;
        public int ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        private string userName;
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        private string password;

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        private string isAdmin;

        public string IsAdmin
        {
            get
            {
                return this.isAdmin;
            }
            set
            {
                this.isAdmin = value;
            }
        }

        public User(int id, string userName, string isAdmin)
        {
            this.id = id;
            this.userName = userName;
            this.isAdmin = isAdmin;
        }

        public User()
        {
            
        }
    }
}
