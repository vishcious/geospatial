using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Core;
using MbUnit.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;

namespace GIS.ArcGIS.UnitTests
{
    [TestFixture]
    public class FeatureClassUtilsTests
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

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mdb", "cdotsde_DBO_ROUTES")]
        public void DoesFeatureClassExist_Test(string subDirectory, string pgdbFilename, string featureclassName)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string pgdb = System.IO.Path.Combine(temp, pgdbFilename);

            IWorkspace workspace = GeodatabaseUtil.GetAccessWorkspace(pgdb);

            Assert.IsTrue(FeatureClassUtil.DoesFeatureClassExist(workspace, featureclassName, true), "Unable to open FeatureClass inside a workspace");
        }

        [RowTest]
        [MbUnit.Framework.Row("Data", "TestData.mdb", "cdotsde_DBO_ROUTES")]
        public void OpenFeatureClass_Test(string subDirectory, string pgdbFilename, string featureclassName)
        {
            string temp = System.IO.Path.Combine(_dataDirectory, subDirectory);
            string pgdb = System.IO.Path.Combine(temp, pgdbFilename);

            IWorkspace workspace = GeodatabaseUtil.GetAccessWorkspace(pgdb);
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;

            IFeatureClass featureClass = FeatureClassUtil.OpenFeatureClass(featureWorkspace, featureclassName);
            Assert.IsNotNull(featureClass, "Unable to open Feature class '" + featureclassName + "' in the PGDB '" + pgdbFilename + "'.");
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
