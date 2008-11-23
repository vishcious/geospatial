using System;
using MbUnit.Framework;
using System.Data;
using GIS.Framework;
using GIS.Framework.SQL2008;
using Microsoft.SqlServer.Types;

namespace GIS.SQL2008.Tests
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="SQl2008GISFields"/> class.
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(SQL2008GISFields))]
    public class SQL2008GISFieldsTest
    {
        #region Fields, SetUp and TearDown
        private SQL2008GISFields target = null;
        
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
            DataTable dt = new DataTable( "GeographyTable" );
            DataColumn dc1 = new DataColumn( "ID", typeof( int ) );
            DataColumn dc2 = new DataColumn( "SHAPE", typeof( SqlGeography ) );
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);

            DataColumn[] list = new DataColumn[]{dc1, dc2};
            target = new SQL2008GISFields(list);

            int count = 0;
            foreach(DataColumn dc in target)
            {
                count++;
            }

            Assert.AreEqual( 1, target.AttributeFieldCount, "The number of columns does not match." );
            Assert.AreEqual( 1, count, "The number of columns does not match." );
            Assert.AreEqual( "GeometryCollection", target.GeometryType, "The geometry column type does not match." );
        }

        #endregion    
    }
}
	
