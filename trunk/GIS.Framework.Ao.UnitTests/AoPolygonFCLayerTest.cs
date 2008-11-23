using System;
using MbUnit.Framework;
using GIS.ArcGIS;
using GIS.Framework;
using GIS.Framework.Ao;
using GIS.Framework.Ao.Layers;
using GIS.Framework.Ao.Features;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Text;
using System.IO;
using Jayrock.Json;

namespace GIS.Framework.Ao.UnitTests
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="AoPolygonFCLayer"/> class.
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(AoPolygonFCLayer))]
    public class AoPolygonFCLayerTest
    {
        IAoInitialize license;
        string _dataDirectory = null;

        #region Fields, SetUp and TearDown
        private AoPolygonFCLayer target = null;
        
        /// <summary>
        /// Sets up the fixture
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            license = new AoInitializeClass();
            license.Initialize(esriLicenseProductCode.esriLicenseProductCodeArcEditor);

            _dataDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
        /// <summary>
        /// Cleans up the fixture
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            license.Shutdown();

            IDisposable disposable = this.target as IDisposable;
            if (disposable!=null)
                disposable.Dispose();
        }
        #endregion
    
        #region Test cases

        [RowTest]
        [MbUnit.Framework.Row("Data", "States_Generalized_w_InstCap")]
        public void ToJSONTest(string subDirectory, string shapefile)
        {
            string shapefileDirectory = System.IO.Path.Combine(_dataDirectory, subDirectory);
            IWorkspace workspace = GeodatabaseUtil.GetShapefileWorkspace(shapefileDirectory);
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(shapefile);

            target = new AoPolygonFCLayer(featureClass, "USStates", "FID");
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "FID < 10";
            target.Search(null);

            StringBuilder text = new StringBuilder();
            StringWriter writer = new StringWriter(text);
            JsonTextWriter jwriter = new JsonTextWriter(writer);
            target.ToJSON(jwriter);
            string result = text.ToString();
            Assert.IsTrue(!String.IsNullOrEmpty(result), "Serialization of Polygon FC layer failed");
        }

        [RowTest]
        [MbUnit.Framework.Row( "Data", "States_Generalized_w_InstCap" )]
        public void ToXMLTest( string subDirectory, string shapefile )
        {
            string shapefileDirectory = System.IO.Path.Combine( _dataDirectory, subDirectory );
            IWorkspace workspace = GeodatabaseUtil.GetShapefileWorkspace( shapefileDirectory );
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            IFeatureClass featureClass = featureWorkspace.OpenFeatureClass( shapefile );

            target = new AoPolygonFCLayer( featureClass, "USStates", "FID" );
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "FID < 10";
            target.Search( null );

            StringBuilder text = new StringBuilder();
            StringWriter writer = new StringWriter( text );
            System.Xml.XmlWriter xwriter = System.Xml.XmlWriter.Create( writer );
            target.ToXML( xwriter );
            string result = text.ToString();
            Assert.IsTrue( !String.IsNullOrEmpty( result ), "Serialization of Polygon FC layer failed" );
        }

        #endregion    
    }
}
	