using System;
using MbUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using GIS.SQL2008;
using Microsoft.SqlServer.Types;
using GIS.Framework.SQL2008;

namespace GIS.SQL2008.Tests
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="SQL2008Database"/> class.
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(SQL2008Database))]
    public class SQL2008DatabaseTest
    {
        #region Fields, SetUp and TearDown
        private SQL2008Database target = null;
        
        /// <summary>
        /// Sets up the fixture
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            
        }
        /// <summary>
        /// Cleans up the fixture
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            IDisposable disposable = this.target as IDisposable;
            if (disposable!=null)
                disposable.Dispose();
        }
        #endregion
    
        #region Test cases
        [Test]
        public void CreateTest()
        {
            string connStr = @"Data Source=vish-laptop\sqlexpress2008;Initial Catalog=VishDB;Integrated Security=True;Connection Timeout=180;";
            target = new SQL2008Database( connStr );
            Assert.IsNotNull( "Failed to create the connection to the sql server 2008 connection." );
            target.Dispose();
        }


        [RowTest]
        [MbUnit.Framework.Row( "vish-laptop\\sqlexpress2008", "VishDB" )]
        public void CreateTest1(string serverName, string databaseName)
        {
            ServerConnection conn = new ServerConnection( serverName );
            SQL2008Server server = new SQL2008Server( conn );
            target = new SQL2008Database( server, databaseName );
            Assert.IsNotNull( target, "Failed to create SQL2008Database" );
            target.Dispose();
        }

        [RowTest]
        [MbUnit.Framework.Row( "vish-laptop\\sqlexpress2008", "VishDB", "VishTable" )]
        public void CreateDeleteTableTest( string serverName, string databaseName, string tableName )
        {
            ServerConnection conn = new ServerConnection( serverName );
            SQL2008Server server = new SQL2008Server( conn );
            target = new SQL2008Database( server, databaseName );
            Assert.IsNotNull( target, "Failed to create SQL2008Database" );

            target.CreateTable( tableName, new List<DataColumn>() { new DataColumn( "ID", typeof( int ) ) }.GetEnumerator() );

            target.DeleteTable( tableName );

            target.Dispose();
        }

        [RowTest]
        [MbUnit.Framework.Row( "vish-laptop\\sqlexpress2008", "VishDB", "VishTable" )]
        public void ContainsTableTest( string serverName, string databaseName, string tableName )
        {
            ServerConnection conn = new ServerConnection( serverName );
            SQL2008Server server = new SQL2008Server( conn );
            target = new SQL2008Database( server, databaseName );
            Assert.IsNotNull( target, "Failed to create SQL2008Database" );

            target.CreateTable( tableName, new List<DataColumn>() { new DataColumn( "ID", typeof( int ) ) }.GetEnumerator() );

            Assert.AreEqual( true, target.ContainsTable( tableName ), "Unable to find table '" + tableName + "' in the database '" + databaseName + "'." );

            target.DeleteTable( tableName );

            target.Dispose();
        }

        [RowTest]
        [MbUnit.Framework.Row( "vish-laptop\\sqlexpress2008", "VishDB", "GeographyTable" )]
        public void CreateGeographyTableTest( string serverName, string databaseName, string tableName )
        {
            ServerConnection conn = new ServerConnection( serverName );
            SQL2008Server server = new SQL2008Server( conn );
            target = new SQL2008Database( server, databaseName );
            Assert.IsNotNull( target, "Failed to create SQL2008Database" );

            target.CreateGeographyTable( tableName, new List<DataColumn>() { new DataColumn( "ID", typeof( int ) ) }.GetEnumerator(), "SHAPE" );

            target.DeleteTable(tableName);

            target.Dispose();
        }

        [RowTest]
        [MbUnit.Framework.Row( "vish-laptop\\sqlexpress2008", "VishDB", "GeometryTable" )]
        public void CreateGeometryTableTest( string serverName, string databaseName, string tableName )
        {
            ServerConnection conn = new ServerConnection( serverName );
            SQL2008Server server = new SQL2008Server( conn );
            target = new SQL2008Database( server, databaseName );
            Assert.IsNotNull( target, "Failed to create SQL2008Database" );

            target.CreateGeometryTable( tableName, new List<DataColumn>() { new DataColumn( "ID", typeof( int ) ) }.GetEnumerator(), "SHAPE" );

            target.DeleteTable( tableName );

            target.Dispose();
        }

        [RowTest]
        [MbUnit.Framework.Row( "vish-laptop\\sqlexpress2008", "VishDB", "GeographyTable", "Shape" )]
        public void CreateGeographyfromGISFieldsTest( string serverName, string databaseName, string tableName , string geometryFieldName)
        {

            DataColumn dc1 = new DataColumn( "ID", typeof( int ) );
            DataColumn dc2 = new DataColumn( "SHAPE", typeof( SqlGeography ) );

            DataColumn[] list = new DataColumn[] { dc1, dc2 };
            SQL2008GISFields gisFields = new SQL2008GISFields( list );

            ServerConnection conn = new ServerConnection( serverName );
            SQL2008Server server = new SQL2008Server( conn );
            target = new SQL2008Database(server, databaseName);

            target.CreateGeographyTable(gisFields, tableName, geometryFieldName);

            target.DeleteTable( tableName );
        }

        #endregion    
    }
}
	