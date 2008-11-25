using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace ArcToSQL2008
{
    public class SMOHelper
    {

        #region Fields

        private string serverName = string.Empty;
        private string instance = string.Empty;
        private string version = string.Empty;
        private bool isLocal;
        private Server server = null;
        private string userName = string.Empty;
        private string password = string.Empty;
        private bool useWindowsAuthentication = true;
        private string testFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        #endregion

        #region Properties

        public bool IsLocal
        {
            get { return isLocal; }
            set { isLocal = value; }
        }
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string Instance
        {
            get { return instance; }
            set { instance = value; }
        }
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }
        public string ServerAndInstanceName
        {
            get
            {
                if (this.instance.Length > 0)
                    return this.serverName + @"\" + this.instance;
                else
                    return this.serverName;
            }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public bool UseWindowsAuthentication
        {
            get { return useWindowsAuthentication; }
            set { useWindowsAuthentication = value; }
        }

        public Server Server
        {
            get { return server; }
        }

        #endregion

        #region Constructors

        public SMOHelper()
        {
            this.server = new Server();
        }

        public SMOHelper(string serverAndInstanceName, string userName, string password, bool useWindowsAuthentication) 
        {
            this.server = new Server();
            if (serverAndInstanceName.Contains(@"\"))
            {
                int slashPos = serverAndInstanceName.IndexOf('\\');
                this.serverName = serverAndInstanceName.Substring(0, slashPos);
                this.instance = serverAndInstanceName.Substring(slashPos + 1);
            }
            else
            {
                serverName = serverAndInstanceName;
                instance = string.Empty;
            }
            this.userName = userName;
            this.password = password;
            this.useWindowsAuthentication = useWindowsAuthentication;
        }

        #endregion

        #region Methods

        public void Connect()
        {
            server.ConnectionContext.ServerInstance = ServerAndInstanceName;
            if (this.useWindowsAuthentication)
            {
                server.ConnectionContext.LoginSecure = useWindowsAuthentication;
            }
            else
            {
                server.ConnectionContext.LoginSecure = this.useWindowsAuthentication;
                server.ConnectionContext.Login = this.userName;
                server.ConnectionContext.Password = this.password;
            }
            server.ConnectionContext.Connect();
        }

        #region Get Lists

        //public void foo()
        //{
        //    //  Get a list of SQL servers available on the network
        //    DataTable dtServers = SmoApplication.EnumAvailableSqlServers(false);
        //    foreach (DataRow row in dtServers.Rows)
        //    {
        //        string sqlServerName = row["Server"].ToString();
        //        if (row["Instance"] != null && row["Instance"].ToString().Length > 0)
        //            sqlServerName += @"\" + row["Instance"].ToString();
        //    }
        //}

        public List<string> GetDatabaseNameList()
        {
            List<string> dbList = new List<string>();
            foreach (Database db in server.Databases)
                dbList.Add(db.Name);
            return dbList;
        }

        public List<string> GetTableNameList(Database db)
        {
            List<string> tableList = new List<string>();
            foreach (Table table in db.Tables)
                tableList.Add(table.Name);
            return tableList;
        }

        public List<string> GetStoredProcedureNameList(Database db)
        {
            List<string> storedProcedureNameList = new List<string>();
            foreach (StoredProcedure storedProcedure in db.StoredProcedures)
                storedProcedureNameList.Add(storedProcedure.Name);
            return storedProcedureNameList;
        }

        public List<string> GetUserNameList(Database db)
        {
            List<string> userNameList = new List<string>();
            foreach (User user in db.Users)
                userNameList.Add(user.Name);
            return userNameList;
        }

        public List<string> GetViewNameList(Database db)
        {
            List<string> viewNameList = new List<string>();
            foreach (View view in db.Views)
                viewNameList.Add(view.Name);
            return viewNameList;
        }

        public List<string> GetColumnNameList(Table table)
        {
            List<string> columnList = new List<string>();
            foreach (Column column in table.Columns)
                columnList.Add(column.Name);
            return columnList;
        }

        #endregion

        #endregion

    }
}
