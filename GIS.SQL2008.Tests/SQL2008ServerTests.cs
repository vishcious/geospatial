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

namespace GIS.SQL2008.Tests
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="SQL2008Server"/> class.
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(SQL2008Server))]
    public class SQL2008ServerTest
    {
        #region Fields, SetUp and TearDown
        private SQL2008Server target = null;
        
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
            SqlConnection conn = new SqlConnection( connStr );
            target = new SQL2008Server( conn );
            Assert.IsNotNull( target, "Failed to create SQL2008Server" );
        }


        [Test]
        public void CreateTest1()
        {
            ServerConnection conn = new ServerConnection( "vish-laptop\\sqlexpress2008" );
            target = new SQL2008Server(conn);
            Assert.IsNotNull( target, "Failed to create SQL2008Server" );
        }


        [RowTest]
        [MbUnit.Framework.Row( "vish-laptop\\sqlexpress2008", "VishDB" )]
        public void ContainsDatabaseTest(string serverName, string databaseName)
        {
            ServerConnection conn = new ServerConnection( serverName );
            target = new SQL2008Server( conn );
            Assert.IsNotNull( target, "Failed to create SQL2008Server" );

            Assert.AreEqual( true, target.ContainsDatabase( databaseName ), "Failed to find VishDB database" );

            Assert.AreEqual( false, target.ContainsDatabase( "ThisDBdoesNotExist" ), "Failed to find VishDB database" );
        }
	
	
        #endregion    
    }
}
	