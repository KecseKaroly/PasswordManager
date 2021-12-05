using System;
using System.Collections.Generic;
using System.Text;

namespace PwManager.Entities
{
    class Accounts
    {
        private int accId;
        public int AccId
        {
            get
            {
                return this.accId;
            }
            set
            {
                this.accId = value;
            }
        }

        private int appId;
        public int AppId
        {
            get
            {
                return this.appId;
            }
            set
            {
                this.appId = value;
            }
        }

        private string username;
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
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



        public Accounts(int appId, string username, string password)
        {
            this.appId = appId;
            this.username = username;
            this.password = password;
        }
        public Accounts()
        {
            
        }
    }
}
