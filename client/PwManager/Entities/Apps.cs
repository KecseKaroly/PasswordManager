using System;
using System.Collections.Generic;
using System.Text;

namespace PwManager.Entities
{
    class Apps
    {
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

        private int userId;
        public int UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
        }

        private string appName;
        public string AppName
        {
            get
            {
                return this.appName;
            }
            set
            {
                this.appName = value;
            }
        }

        public Apps( int appId, int userId, string appName)
        {
            this.appId = appId;
            this.userId = userId;
            this.appName = appName;
        }
        public Apps()
        {
            
        }
    }
}
