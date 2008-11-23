using System;
using MbUnit.Framework;
using GIS.Framework.SQL2008.SQLGeometry;
using GIS.Framework.SQL2008.SQLGeometry.Features;
using GIS.Framework.SQL2008.SQLGeometry.Layers;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using Jayrock.Json;

namespace GIS.Framework.SQL2008.UnitTests.SQLGeometry
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="SQL2008PointLayer"/> class.
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(SQL2008PointLayer))]
    public class SQL2008PointLayerTest
    {
        #region Fields, SetUp and TearDown
        private SQL2008PointLayer target = null;
        
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
        public void Test()
        {
            
        }
        #endregion    
    }
}
	