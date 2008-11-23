using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Core;
using MbUnit.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

namespace GIS.ArcGIS.UnitTests
{
    [TestFixture]
    [TestsOn(typeof(GeodatabaseUtil))]
    public class GeodatabaseUtilTests
    {
        IAoInitialize license;
        string _dataDirectory = null;

        [SetUp]
        public void SetUp()
        {
            // set up the fixture
            license = new AoInitializeClass();
            license.Initialize(esriLicenseProductCode.esriLicenseProductCodeArcView);

            _dataDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        #region "Tests"

        [Test]
        public void GetShapefileWorkspace_Test_Success()
        {
            IWorkspace workspace = GeodatabaseUtil.GetShapefileWorkspace(_dataDirectory);

            Assert.AreNotEqual(workspace, null, "Did not open a shapefile workspace for a valid directory");
        }

        [RowTest]
        [MbUnit.Framework.Row("C:\\vish123")]
        public void GetShapefileWorkspace_Test_Failure(string shapefileDirectory)
        {
            IWorkspace workspace = GeodatabaseUtil.GetShapefileWorkspace(shapefileDirectory);

            Assert.AreEqual(workspace, null, "Did not fail when opening a shapefile workspace for an invalid directory");
        }

        [RowTest]
        [MbUnit.Framework.Row("PGDB.mdb")]
        public void CreatePersonalGeodatabase_Test_Success(string pgdbFileName)
        {
            string pgdbFile = System.IO.Path.Combine(_dataDirectory, pgdbFileName);
            if (File.Exists(pgdbFile))
                File.Delete(pgdbFile);
            GeodatabaseUtil.CreatePersonalGeodatabase(pgdbFile);
        }

        [RowTest]
        [MbUnit.Framework.Row("PGDB1.mdb")]
        public void TestCreateFeatureClassWithValidArguments_Test(string pgdbFileName)
        {
            string pgdbFile = System.IO.Path.Combine(_dataDirectory, pgdbFileName);
            if (File.Exists(pgdbFile))
                File.Delete(pgdbFile);
            GeodatabaseUtil.CreatePersonalGeodatabase(pgdbFile);

            IWorkspaceFactory pgdbWSFactory = new AccessWorkspaceFactoryClass();
            IWorkspace workspace = pgdbWSFactory.OpenFromFile(pgdbFile, 0);

            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_NAD1983);
            spatialReference.SetDomain(-5120900, 900714804574.099, -9998100, 900709927374.099);

            GeodatabaseUtil.CreateFeatureClass(workspace, "Test", spatialReference, esriFeatureType.esriFTSimple, esriGeometryType.esriGeometryPoint, null, null, null, null);
        }

        [RowTest]
        [MbUnit.Framework.Row("PGDB2.mdb")]
        public void TestCreateTableWithValidArguments_Test(string pgdbFileName)
        {
            string pgdbFile = System.IO.Path.Combine(_dataDirectory, pgdbFileName);
            if (File.Exists(pgdbFile))
                File.Delete(pgdbFile);
            GeodatabaseUtil.CreatePersonalGeodatabase(pgdbFile);

            IWorkspaceFactory pgdbWSFactory = new AccessWorkspaceFactoryClass();
            IWorkspace workspace = pgdbWSFactory.OpenFromFile(pgdbFile, 0);
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;

            GeodatabaseUtil.CreateTable(featureWorkspace, "Test", null);
        }

        [RowTest]
        [MbUnit.Framework.Row("PGDB3.mdb")]
        public void TestGetAccessWorkspace(string pgdbFileName)
        {
            string pgdbFile = System.IO.Path.Combine(_dataDirectory, pgdbFileName);
            if (File.Exists(pgdbFile))
                File.Delete(pgdbFile);
            GeodatabaseUtil.CreatePersonalGeodatabase(pgdbFile);

            Assert.IsNotNull(GeodatabaseUtil.GetAccessWorkspace(pgdbFile), "Unable to open workspace for an access personal geodatabase");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mdb")]
        public void GetAccessWorkspace_Test_Success(string subDirectory, string pgdbFileName)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string pgdbFile = System.IO.Path.Combine(temp, pgdbFileName);
            Assert.IsNotNull(GeodatabaseUtil.GetAccessWorkspace(pgdbFile), "Unable to open workspace for the '" + pgdbFile + "' personal geodatabase");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mdb")]
        public void GetPGDBWorkspaceName_Test(string subDirectory, string pgdbFileName)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string pgdbFile = System.IO.Path.Combine(temp, pgdbFileName);

            IWorkspaceName workspaceName = GeodatabaseUtil.GetPGDBWorkspaceName(pgdbFile);
            Assert.IsNotNull(workspaceName, "Unable to get the IWorkspaceName for the pgdb file '" + pgdbFile + "'.");

            IName name = (IName)workspaceName;
            if (name != null)
                Assert.IsNotNull(name.Open(), "Workspace name obtained was invalid.");
            else
                Assert.Fail("Unable to run test. A valid IName reference could not be obtained.");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mdb", "cdotsde_DBO_ROUTES")]
        public void GetFeatureClassName_Test(string subDirectory, string pgdbFileName, string featureClassName)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string pgdbFile = System.IO.Path.Combine(temp, pgdbFileName);

            IFeatureClassName fcName = GeodatabaseUtil.GetFeatureClassName(GeodatabaseUtil.GetPGDBWorkspaceName(pgdbFile), featureClassName);
            Assert.IsNotNull(fcName, "Unable to get the IFeatureClassName for the FeatureClass '" + featureClassName + "' in the pgdb file '" + pgdbFile + "'.");

            IName name = (IName)fcName;
            if (name != null)
                Assert.IsNotNull(name.Open(), "FeatureClass name obtained was invalid.");
            else
                Assert.Fail("Unable to run test. A valid IName reference could not be obtained.");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mdb")]
        public void GetWorkspaceName_Test(string subDirectory, string pgdbFileName)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string pgdbFile = System.IO.Path.Combine(temp, pgdbFileName);

            IWorkspace workspace = GeodatabaseUtil.GetAccessWorkspace(pgdbFile);

            if (workspace != null)
                Assert.IsNotNull(GeodatabaseUtil.GetWorkspaceName(workspace), "Unable to obtain the workspace name from the workspace.");
            else
                Assert.Fail("Unable to run test. A valid workspace input argument cannot be obtained.");
        }

        [RowTest]
        [MbUnit.Framework.Row("PGDB4.mdb")]
        public void TestCreateFeatureClassWithValidArguments(string pgdbFileName)
        {
            string pgdbFile = System.IO.Path.Combine(_dataDirectory, pgdbFileName);
            if (File.Exists(pgdbFile))
                File.Delete(pgdbFile);
            GeodatabaseUtil.CreatePersonalGeodatabase(pgdbFile);

            IWorkspaceFactory pgdbWSFactory = new AccessWorkspaceFactoryClass();
            IWorkspace workspace = pgdbWSFactory.OpenFromFile(pgdbFile, 0);

            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_NAD1983);
            spatialReference.SetDomain(-5120900, 900714804574.099, -9998100, 900709927374.099);

            GeodatabaseUtil.CreateFeatureClass(workspace, "Test", spatialReference, esriFeatureType.esriFTSimple, esriGeometryType.esriGeometryPoint, null, null, null, null);
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mxd", "Layers")]
        public void GetMap_Test(string subDirectory, string mapDocumentFilename, string dataframeName)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string map = System.IO.Path.Combine(temp, mapDocumentFilename);

            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.Open(map, null);

            Assert.IsNotNull(GeodatabaseUtil.GetMap(mapDocument, dataframeName));
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mxd", "Layers", 6)]
        public void GetLayers_FromMap_Test(string subDirectory, string mapDocumentFilename, string dataframeName, int expectedLayerCount)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string mapDoc = System.IO.Path.Combine(temp, mapDocumentFilename);

            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.Open(mapDoc, null);

            IMap map = GeodatabaseUtil.GetMap(mapDocument, dataframeName);
            List<ILayer> layers = GeodatabaseUtil.GetLayers(map);
            Assert.IsNotNull(layers, "Unable to obtain layers from map");
            Assert.AreEqual(expectedLayerCount, layers.Count, "Number of layers from map do not match expected value.");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mxd", "Layers", 1)]
        public void GetGroupLayers_FromMap_Test(string subDirectory, string mapDocumentFilename, string dataframeName, int expectedLayerCount)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string mapDoc = System.IO.Path.Combine(temp, mapDocumentFilename);

            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.Open(mapDoc, null);

            IMap map = GeodatabaseUtil.GetMap(mapDocument, dataframeName);
            List<IGroupLayer> layers = GeodatabaseUtil.GetGroupLayers(map);
            Assert.IsNotNull(layers, "Unable to obtain layers from map");
            Assert.AreEqual(expectedLayerCount, layers.Count, "Number of layers from map do not match expected value.");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mxd", "Layers", 5)]
        public void GetFeatureLayers_FromMap_Test(string subDirectory, string mapDocumentFilename, string dataframeName, int expectedLayerCount)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string mapDoc = System.IO.Path.Combine(temp, mapDocumentFilename);

            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.Open(mapDoc, null);

            IMap map = GeodatabaseUtil.GetMap(mapDocument, dataframeName);
            List<IFeatureLayer> layers = GeodatabaseUtil.GetFeatureLayers(map);
            Assert.IsNotNull(layers, "Unable to obtain layers from map");
            Assert.AreEqual(expectedLayerCount, layers.Count, "Number of layers from map do not match expected value.");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mxd", "Layers", 6)]
        public void GetLayers_FromDocument_Test(string subDirectory, string mapDocumentFilename, string dataframeName, int expectedLayerCount)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string mapDoc = System.IO.Path.Combine(temp, mapDocumentFilename);

            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.Open(mapDoc, null);

            List<ILayer> layers = GeodatabaseUtil.GetLayers(mapDocument, dataframeName);
            Assert.IsNotNull(layers, "Unable to obtain layers from map");
            Assert.AreEqual(expectedLayerCount, layers.Count, "Number of layers from map do not match expected value.");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mxd", "Layers", 1)]
        public void GetGroupLayers_FromDocument_Test(string subDirectory, string mapDocumentFilename, string dataframeName, int expectedLayerCount)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string mapDoc = System.IO.Path.Combine(temp, mapDocumentFilename);

            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.Open(mapDoc, null);

            List<IGroupLayer> layers = GeodatabaseUtil.GetGroupLayers(mapDocument, dataframeName);
            Assert.IsNotNull(layers, "Unable to obtain layers from map");
            Assert.AreEqual(expectedLayerCount, layers.Count, "Number of layers from map do not match expected value.");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mxd", "Layers", 5)]
        public void GetFeatureLayers_FromDocument_Test(string subDirectory, string mapDocumentFilename, string dataframeName, int expectedLayerCount)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string mapDoc = System.IO.Path.Combine(temp, mapDocumentFilename);

            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.Open(mapDoc, null);

            List<IFeatureLayer> layers = GeodatabaseUtil.GetFeatureLayers(mapDocument, dataframeName);
            Assert.IsNotNull(layers, "Unable to obtain layers from map");
            Assert.AreEqual(expectedLayerCount, layers.Count, "Number of layers from map do not match expected value.");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mdb", "cdotsde_DBO_ROUTES")]
        public void TransferData_Test(string subDirectory, string inputPGDBFile, string inputFeatureClassName)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string pgdb = System.IO.Path.Combine(temp, inputPGDBFile);

            IWorkspace inWorkspace = GeodatabaseUtil.GetAccessWorkspace(pgdb);
            IFeatureWorkspace inFeatureWorkspace = (IFeatureWorkspace)inWorkspace;
            IFeatureClass inFeatureClass = FeatureClassUtil.OpenFeatureClass(inFeatureWorkspace, inputFeatureClassName);
            IGeoDataset geoDataset = (IGeoDataset)inFeatureClass;

            string[] files = Directory.GetFiles(temp, inputFeatureClassName + ".*", SearchOption.TopDirectoryOnly);

            foreach (string item in files)
            {
                File.Delete(item);
            }

            IWorkspace outWorkspace = GeodatabaseUtil.GetShapefileWorkspace(temp);
            IFeatureWorkspace outFeatureWorkspace = (IFeatureWorkspace)outWorkspace;
            IWorkspaceName outWorkspaceName = GeodatabaseUtil.GetWorkspaceName(outWorkspace);
            IFeatureClassName outFeatureclassName = GeodatabaseUtil.GetFeatureClassName(outWorkspaceName, inputFeatureClassName);

            IEnumInvalidObject invalidObjects = GeodatabaseUtil.TransferData(inFeatureWorkspace, inFeatureClass, outFeatureWorkspace, outFeatureclassName, null, null, geoDataset.SpatialReference);
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mxd", "Layers", "temp")]
        public void TransferMapDataWithinExtents_Test(string subDirectory, string mapDocumentFilename, string dataframeName, string outputShapefileDirectory)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string mapDoc = System.IO.Path.Combine(temp, mapDocumentFilename);

            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.Open(mapDoc, null);

            IMap map = GeodatabaseUtil.GetMap(mapDocument, dataframeName);
            List<IFeatureLayer> layers = GeodatabaseUtil.GetFeatureLayers(map);
            IDataset dataset = (IDataset)layers[0].FeatureClass;
            IGeoDataset geoDataset = (IGeoDataset)layers[0].FeatureClass;

            string destination = System.IO.Path.Combine(temp, outputShapefileDirectory);
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }
            Directory.CreateDirectory(destination);

            IWorkspace outWorkspace = GeodatabaseUtil.GetShapefileWorkspace(destination);
            IFeatureWorkspace outFeatureWorkspace = (IFeatureWorkspace)outWorkspace;
            IWorkspaceName outWorkspaceName = GeodatabaseUtil.GetWorkspaceName(outWorkspace);
            IFeatureClassName outFeatureclassName = GeodatabaseUtil.GetFeatureClassName(outWorkspaceName, dataset.Name);

            //ESRI.ArcGIS.ADF.Web.Geometry.Envelope envelope = new ESRI.ArcGIS.ADF.Web.Geometry.Envelope(227884.141, 4167884.377, 602406.528, 4470244.455);
            ESRI.ArcGIS.ADF.Web.Geometry.Envelope envelope = new ESRI.ArcGIS.ADF.Web.Geometry.Envelope(445092.693, 4360557.744, 541068.565, 4418287.592);

            Dictionary<int, KeyValuePair<string, IEnumInvalidObject>> invalidObjects = GeodatabaseUtil.TransferMapDataWithinExtents(map, outFeatureWorkspace, null, envelope, geoDataset.SpatialReference);

        }

        #endregion

        [TearDown]
        public void TearDown()
        {
            // clean up
            license.Shutdown();
        }
    }
}
