using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.FDO.Connections;
using GIS.Common;
using System.IO;
using OSGeo.FDO.ClientServices;
using OSGeo.FDO;

namespace GIS.FDO
{
    public static class FDOShpUtil
    {
        public IConnection OpenShapefileConnection( string shpDirectory )
        {
            return OpenShapefileConnection( "OSGeo.SHP.3.3", shpDirectory );
        }

        public IConnection OpenShapefileConnection( string providerName, string shpDirectory )
        {
            providerName.RequireArgument<string>( "providerName" ).NotNullOrEmpty();
            shpDirectory.RequireArgument<string>( "shpDirectory" ).NotNullOrEmpty();
            if( !Directory.Exists( shpDirectory ) )
                throw new ArgumentException( string.Format("The directory {0} is either invalid or could not be found.", shpDirectory) );

            IConnectionManager connectionManager = FeatureAccessManager.GetConnectionManager();
            IConnection conn = connectionManager.CreateConnection( providerName );
            conn.ConnectionString = @"DefaultFileLocation=" + shpDirectory;
            ConnectionState state = conn.Open();

            return conn;
        }
    }
}
