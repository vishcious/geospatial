using System;
using MbUnit.Framework;
using GIS.Framework;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using GIS.ArcGIS;

namespace GIS.ArcGIS.UnitTests
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="AoGISFields"/> class.
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(AoGISFields))]
    public class AoGISFieldsTest
    {
        IAoInitialize license;
        string _dataDirectory = null;

        #region Fields, SetUp and TearDown
        private AoGISFields target = null;
        
        /// <summary>
        /// Sets up the fixture
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            license = new AoInitializeClass();
            license.Initialize( esriLicenseProductCode.esriLicenseProductCodeArcEditor );

            _dataDirectory = System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location );
        }
        /// <summary>
        /// Cleans up the fixture
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            license.Shutdown();

            IDisposable disposable = this.target as IDisposable;
            if( disposable != null )
                disposable.Dispose();
        }
        #endregion
    
        #region Test cases
        
        [RowTest]
        [MbUnit.Framework.Row( "Data", "States_Generalized_w_InstCap" )]
        public void CreateTest(string subDirectory, string shapefile )
        {
            string shapefileDirectory = System.IO.Path.Combine( _dataDirectory, subDirectory );
            IWorkspace workspace = GeodatabaseUtil.GetShapefileWorkspace( shapefileDirectory );
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            IFeatureClass featureClass = featureWorkspace.OpenFeatureClass( shapefile );

            AoGISFields fields = new AoGISFields( featureClass.Fields );
            Assert.IsNotNull( fields, "Creating 'AoGISFields' failed." );
            Assert.IsNotNull( fields.GeometryType, "The geometry column was not found" );
            Assert.AreEqual( fields.GeometryType, "MultiPolygon", "The geometry type does not match." );
            Assert.AreEqual( fields.AttributeFieldCount, 11, "The attribute field count does not match" );
        }

        #endregion    
    }
}
	