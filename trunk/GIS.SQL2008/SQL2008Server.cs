using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace GIS.SQL2008
{
    public class SQL2008Server: IDisposable
    {
        ServerConnection _serverConnection;
        Server _server;

        public SQL2008Server( SqlConnection sqlConnection )
        {
            if( sqlConnection == null )
                throw new ArgumentNullException( "sqlConnection" );

            if( sqlConnection.State == ConnectionState.Broken )
                throw new ArgumentException( "The sql connection provided is in a broken state.", "sqlConnection" );

            if(sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();

            _serverConnection = new ServerConnection( sqlConnection );
            _server = new Server( _serverConnection );
        }

        public SQL2008Server( ServerConnection serverConnection )
        {
            if( serverConnection == null )
                throw new ArgumentNullException( "serverConnection" );

            _serverConnection = serverConnection;
            _server = new Server( _serverConnection );
        }

        #region Public Properties

        public Server Server
        {
            get
            {
                return _server;
            }
        }

        #endregion

        #region Public Methods

        public bool ContainsDatabase( string dbName )
        {
            return _server.Databases.Contains( dbName );
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _serverConnection.Disconnect();
            _serverConnection = null;
            _server = null;
        }

        #endregion
    }
}
