using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;

using ESRI.ArcGIS.Carto;

namespace GIS.ArcGIS
{
    public static class GeodatabaseUtil
    {
        public static string GetUnQualifiedName(string browsename)
        {

            if (browsename.Contains("."))
            {

                string[] arName = browsename.Split('.');
                return arName[arName.Length - 1];
            }

            else
            {
                return browsename;
            }
        }

        public static ITable CreateTable(IFeatureWorkspace featureWorkspace, string name, IFields fields)
        {
            if (featureWorkspace == null)
            {
                throw new ArgumentNullException("Error creating table. The reference to IFeatureWorkspace cannot be NULL.");
            }
            UID pCLSID;
            pCLSID = new UIDClass();
            pCLSID.Value = "esriGeoDatabase.Object";
            IFieldsEdit pFieldsEdit;

            if ((fields == null) || (fields.FieldCount == 0))
            {
                fields = new Fields();
                pFieldsEdit = fields as IFieldsEdit;

                IField pFieldOID = new Field();
                IFieldEdit pFieldOIDEdit = pFieldOID as IFieldEdit;
                pFieldOIDEdit.Name_2 = "OBJECTID";
                pFieldOIDEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                pFieldOIDEdit.IsNullable_2 = false;

                pFieldsEdit.AddField(pFieldOID);
            }
            ITable table = null;
            try
            {
                table = featureWorkspace.CreateTable(name, fields, pCLSID, null, "");
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                throw new System.Runtime.InteropServices.COMException("An unexpected error occurred while trying to create a table in the geodatabase", ex);
            }

            // Set the alias
            IClassSchemaEdit classSchemaEdit = (IClassSchemaEdit)table;
            classSchemaEdit.AlterAliasName(GetUnQualifiedName(name));

            // Return the table
            return table;
        }

        public static void CreatePersonalGeodatabase(string destinationPGDBFile)
        {
            string pgdbFileName;
            string pgdbParentDirectory;
            string pgdbFile;

            destinationPGDBFile = System.IO.Path.GetFullPath(destinationPGDBFile);
            pgdbFileName = System.IO.Path.GetFileName(destinationPGDBFile);
            pgdbParentDirectory = System.IO.Path.GetDirectoryName(destinationPGDBFile);
            pgdbFile = System.IO.Path.Combine(pgdbParentDirectory, pgdbFileName);

            IWorkspaceFactory pgdbWSFactory = new AccessWorkspaceFactoryClass();
            IWorkspaceName pgdbWorkspaceName;
            //Create the Access database file if it does not already exist
            if (!System.IO.File.Exists(pgdbFile))
            {
                try
                {
                    pgdbWorkspaceName = pgdbWSFactory.Create(pgdbParentDirectory, pgdbFileName, null, 0);
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    throw new System.Runtime.InteropServices.COMException("An unexpected error occurred while trying to create a personal geodatabase.", e);
                }
            }
            else
            {
                throw new System.IO.IOException("A file with the same name already exists. Unable to create personal geodatabase.");
            }
        }

        public static IFeatureClass CreateFeatureClass(object objectWorkspace, string name, ISpatialReference spatialReference, esriFeatureType featureType, esriGeometryType geometryType, IFields fields, UID uidCLSID, UID uidCLSEXT, string configWord)
        {
            // Check for invalid parameters.
            if (objectWorkspace == null)
                throw new ArgumentNullException("[objectWorkspace] cannot be null");

            if (!((objectWorkspace is IFeatureWorkspace) || (objectWorkspace is IFeatureDataset)))
                throw (new ArgumentNullException("[objectWorkspace] must be IFeatureWorkspace or IFeatureDataset"));

            if (string.IsNullOrEmpty(name))
                throw (new ArgumentNullException("[name] cannot be empty"));

            if ((objectWorkspace is IWorkspace) && (spatialReference == null))
                throw (new ArgumentNullException("[spatialReference] cannot be null for StandAlong FeatureClasses"));


            // Set ClassID (if Null)
            if (uidCLSID == null)
            {
                uidCLSID = new UIDClass();
                switch (featureType)
                {
                    case (esriFeatureType.esriFTSimple):
                        uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                        break;
                    case (esriFeatureType.esriFTSimpleJunction):
                        geometryType = esriGeometryType.esriGeometryPoint;
                        uidCLSID.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTComplexJunction):
                        uidCLSID.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTSimpleEdge):
                        geometryType = esriGeometryType.esriGeometryPolyline;
                        uidCLSID.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTComplexEdge):
                        geometryType = esriGeometryType.esriGeometryPolyline;
                        uidCLSID.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTAnnotation):
                        geometryType = esriGeometryType.esriGeometryPolygon;
                        uidCLSID.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                        break;
                    case (esriFeatureType.esriFTDimension):
                        geometryType = esriGeometryType.esriGeometryPolygon;
                        uidCLSID.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                        break;
                }
            }

            // Set uidCLSEXT (if Null)
            if (uidCLSEXT == null)
            {
                switch (featureType)
                {
                    case (esriFeatureType.esriFTAnnotation):
                        uidCLSEXT = new UIDClass();
                        uidCLSEXT.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                        break;
                    case (esriFeatureType.esriFTDimension):
                        uidCLSEXT = new UIDClass();
                        uidCLSEXT.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                        break;
                }
            }

            // Add Fields
            if (fields == null)
            {
                // Create fields collection
                fields = new FieldsClass();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                // Create the geometry field
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;

                // Assign Geometry Definition
                geometryDefEdit.GeometryType_2 = geometryType;
                geometryDefEdit.GridCount_2 = 1;
                geometryDefEdit.set_GridSize(0, 0.5);
                geometryDefEdit.AvgNumPoints_2 = 2;
                geometryDefEdit.HasM_2 = false;
                geometryDefEdit.HasZ_2 = false;
                if (objectWorkspace is IWorkspace)
                {
                    // If this is a STANDALONE FeatureClass then add spatial reference.
                    geometryDefEdit.SpatialReference_2 = spatialReference;
                }

                // Create OID Field
                IField fieldOID = new FieldClass();
                IFieldEdit fieldEditOID = (IFieldEdit)fieldOID;
                fieldEditOID.Name_2 = "OBJECTID";
                fieldEditOID.AliasName_2 = "OBJECTID";
                fieldEditOID.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldsEdit.AddField(fieldOID);

                // Create Geometry Field
                IField fieldShape = new FieldClass();
                IFieldEdit fieldEditShape = (IFieldEdit)fieldShape;
                fieldEditShape.Name_2 = "SHAPE";
                fieldEditShape.AliasName_2 = "SHAPE";
                fieldEditShape.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEditShape.GeometryDef_2 = geometryDef;
                fieldsEdit.AddField(fieldShape);
            }

            // Locate Shape Field
            string stringShapeFieldName = null;
            int i = 0;
            while (i <= fields.FieldCount - 1)
            {
                if (fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    stringShapeFieldName = fields.get_Field(i).Name;
                    break;
                }
                i = i + 1;
            }
            if (stringShapeFieldName == null)
            {
                throw (new System.Data.ConstraintException("Cannot locate geometry field in FIELDS"));
            }

            IFeatureClass featureClass = null;

            if (objectWorkspace is IFeatureWorkspace)
            {
                // Create a STANDALONE FeatureClass
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)objectWorkspace;

                featureClass = featureWorkspace.CreateFeatureClass(name, fields, uidCLSID, uidCLSEXT, featureType, stringShapeFieldName, configWord);
            }
            else if (objectWorkspace is IFeatureDataset)
            {
                IFeatureDataset featureDataset = (IFeatureDataset)objectWorkspace;
                featureClass = featureDataset.CreateFeatureClass(name, fields, uidCLSID, uidCLSEXT, featureType, stringShapeFieldName, configWord);
            }

            // Set the alias
            IClassSchemaEdit classSchemaEdit = (IClassSchemaEdit)featureClass;
            classSchemaEdit.AlterAliasName(GeodatabaseUtil.GetUnQualifiedName(name));

            // Return FeatureClass
            return featureClass;
        }

        /// <summary>
        /// Gets a reference to an Access workspace object at the specified path.
        /// </summary>
        /// <param name="databasePath"></param>
        /// <returns>AccessWorkspace</returns>
        /// <remarks>
        /// </remarks>
        public static IWorkspace GetAccessWorkspace(string databasePath)
        {
            // Declare COM objects.
            IWorkspaceFactory2 workspaceFactory;
            IPropertySet2 propertyset;

                // Create the Access Workspace factory
                workspaceFactory = new AccessWorkspaceFactoryClass();

                // Create the property set
                propertyset = new PropertySetClass();
                propertyset.SetProperty("Database", databasePath);

                // Get the workspace
                return workspaceFactory.Open(propertyset, 0);
        }

        public static IXMLStream WriteFeatureClassToXMLStream(IFeatureClass featureClass, IQueryFilter queryFilter)
        {
            string ss = "http://www.esri.com/schemas/ArcGIS/9.2 ";
            IPropertySet pPropset = new PropertySet();
            pPropset.SetProperty("Namespaceuri", "http://schemas.esri.com/ArcGis/Server/ ");
            IPropertySet pCpropset = new PropertySet();
            pCpropset.SetProperty("http://schemas.esri.com/ArcGis/Server/", "xmlns");
            pPropset.SetProperty("Namespaceprefixes", pCpropset);

            IRecordSetInit pRecinit = new RecordSetClass();
            pRecinit.SetSourceTable(featureClass as ITable, queryFilter);
            IRecordSet pRec = pRecinit as IRecordSet;
            IXMLStream pXmlStream = new XMLStream();
            IXMLWriter pXmlWriter = new XMLWriter();
            pXmlWriter.WriteTo(pXmlStream as IStream);
            IXMLSerializer pXmlSerial = new XMLSerializer();
            pXmlSerial.WriteObject(pXmlWriter, pPropset, null, "Record", ss, pRec);
            return pXmlStream;
        }

        /// <summary>
        /// Gets the workspace reference to the shapefile directory.
        /// </summary>
        /// <param name="directoryPath">The directory path where the shapefile workspace will work against.</param>
        /// <returns>the workspace reference if directory exists else returns NULL</returns>
        public static IWorkspace GetShapefileWorkspace(string directoryPath)
        {
            if (System.IO.Directory.Exists(directoryPath))
            {
                IPropertySet propSet = new PropertySetClass();
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactoryClass();

                propSet.SetProperty("DATABASE", directoryPath);

                IWorkspace workspace = workspaceFactory.Open(propSet, 0);
                return workspace;
            }
            return null;
        }

        /// <summary>
        /// Transfers data from an input FeatureClass to an output FeatureClass with data filtered with the specified query filter.
        /// </summary>
        /// <param name="inputWorkspace">The input workspace.</param>
        /// <param name="inputFeatureClassName">Name of the input feature class.</param>
        /// <param name="outputWorkspace">The output workspace.</param>
        /// <param name="outputFeatureClassName">Name of the output feature class.</param>
        /// <param name="outputFeatureDatasetName">The output FeatureDataset</param>
        /// <param name="queryFilter">The query filter used to filter data to be transfered.</param>
        public static IEnumInvalidObject TransferData(IFeatureWorkspace inputWorkspace, IFeatureClass inputFeatureClass, IFeatureWorkspace outputWorkspace, IFeatureClassName outputFeatureClassName, IFeatureDatasetName outputFeatureDatasetName, IQueryFilter queryFilter, ISpatialReference outputSpatialReference)
        {
            if (inputWorkspace == null)
                throw new ArgumentNullException("A valid input feature workspace reference [inputWorkspace] is required.");

            if (inputFeatureClass == null)
                throw new ArgumentNullException("A valid input featureclass [inputFeatureClass] is required.");

            if (outputWorkspace == null)
                throw new ArgumentNullException("A valid output feature workspace reference [outputWorkspace] is required.");

            if (outputFeatureClassName == null)
                throw new ArgumentNullException("A valid output featureclass name reference [outputFeatureClassName] is required.");

            IDataset dataset = (IDataset)inputFeatureClass;
            IFeatureClassName inputFeatureClassName = (IFeatureClassName)dataset.FullName;

            IEnumFieldError error = null;
            IFields outFields = null;
            IFields inFields = inputFeatureClass.Fields;
            FieldCheckerClass fieldChecker = new FieldCheckerClass();
            fieldChecker.InputWorkspace = (IWorkspace) inputWorkspace;
            fieldChecker.ValidateWorkspace = (IWorkspace)outputWorkspace;
            fieldChecker.Validate(inFields, out error, out outFields);

            IField shapeField = null;
            for (int i = 0; i < outFields.FieldCount; i++)
            {
                if (outFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    shapeField = outFields.get_Field(i);
                    break;
                }
            }


            IGeometryDef shapeGeoDef = shapeField.GeometryDef;
            IGeometryDefEdit shapeGeoDefEdit = shapeGeoDef as IGeometryDefEdit;
            shapeGeoDefEdit.GridCount_2 = 1;
            shapeGeoDefEdit.set_GridSize(0, DefaultIndexGrid(ref inputFeatureClass));
            shapeGeoDefEdit.SpatialReference_2 = outputSpatialReference;

            FeatureDataConverterClass fdc = new FeatureDataConverterClass();
            return fdc.ConvertFeatureClass(inputFeatureClassName, queryFilter, outputFeatureDatasetName, outputFeatureClassName, shapeGeoDefEdit, outFields, "", 1000, 0);
        }

        public static double DefaultIndexGrid(ref ESRI.ArcGIS.Geodatabase.IFeatureClass InFC)
        {
            int lngNumFeat;
            int lngSampleSize;
            ESRI.ArcGIS.Geodatabase.IFields pFields;
            ESRI.ArcGIS.Geodatabase.IField pField;
            string strFIDName;
            string strWhereClause;
            //int lngCurrFID;
            ESRI.ArcGIS.Geodatabase.IFeature pFeat;
            ESRI.ArcGIS.Geodatabase.IFeatureCursor pFeatCursor;
            ESRI.ArcGIS.Geometry.IEnvelope pFeatEnv;
            ESRI.ArcGIS.Geodatabase.IQueryFilter pQueryFilter;
            List<int> pNewCol = new List<int>();
            int lngKMax;
            double dblMaxDelta;
            dblMaxDelta = 0;
            double dblMinDelta;
            dblMinDelta = 1000000000000;
            double dblSquareness;
            dblSquareness = 1;
            const short SampleSize = 1;
            const short Factor = 1;
            object[] ColInfo = new object[1];
            object[] c0 = new object[4];
            c0[0] = "minext";
            c0[1] = System.Convert.ToInt16(5);
            c0[2] = System.Convert.ToInt16(-1);
            c0[3] = false;
            ColInfo[0] = c0;
            lngNumFeat = InFC.FeatureCount(null) - 1;
            if (lngNumFeat <= 0)
            {
                return 1000;
            }
            if (InFC.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint | InFC.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
            {
                return DefaultIndexGridPoint(ref InFC);
            }
            lngSampleSize = lngNumFeat * SampleSize;
            if (lngSampleSize > 1000)
            {
                lngSampleSize = 1000;
            }
            pFields = InFC.Fields;
            // GET the OBJECTID field
            pField = pFields.get_Field(0);
            for (int m = 0; m < pFields.FieldCount; m++)
            {
                if (pFields.get_Field(m).Name == InFC.OIDFieldName)
                {
                    pField = pFields.get_Field(m);
                    break;
                }
            }

            strFIDName = pField.Name;
            for (int i = 1; i <= lngNumFeat; i += System.Convert.ToInt32(lngNumFeat / lngSampleSize))
            {
                pNewCol.Add(i);
            }
            for (int j = 0; j <= pNewCol.Count - 1; j += 250)
            {
                lngKMax = Min(pNewCol.Count - j, 250);
                strWhereClause = strFIDName + " IN(";
                for (int k = 0; k < lngKMax; k++)
                {
                    strWhereClause = strWhereClause + System.Convert.ToString(pNewCol[(j + k)]) + ",";
                }
                //strWhereClause = Mid(strWhereClause, 1, strWhereClause.Length - 1) + ")";
                strWhereClause = strWhereClause.Substring(0, strWhereClause.Length - 1) + ")";
                pQueryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilter();
                pQueryFilter.WhereClause = strWhereClause;
                pFeatCursor = InFC.Search(pQueryFilter, true);
                pFeat = pFeatCursor.NextFeature();
                while (!(pFeat == null))
                {
                    pFeatEnv = pFeat.Extent;
                    if (!pFeatEnv.IsEmpty)
                    {
                        dblMaxDelta = Max(dblMaxDelta, Max((pFeatEnv.Width), (pFeatEnv.Height)));
                        dblMinDelta = Min(dblMinDelta, Min((pFeatEnv.Width), (pFeatEnv.Height)));
                        if (dblMinDelta != 0)
                        {
                            dblSquareness = dblSquareness + ((Min((pFeatEnv.Width), (pFeatEnv.Height)) / (Max((pFeatEnv.Width), (pFeatEnv.Height)))));
                        }
                        else
                        {
                            dblSquareness = dblSquareness + 0.0001;
                        }
                    }
                    pFeat = pFeatCursor.NextFeature();
                }
            }
            if (((dblSquareness / lngSampleSize) > 0.5))
            {
                return (dblMinDelta + ((dblMaxDelta - dblMinDelta) / 2)) * Factor;
            }
            else
            {
                return (dblMaxDelta / 2) * Factor;
            }
        }

        private static double DefaultIndexGridPoint(ref ESRI.ArcGIS.Geodatabase.IFeatureClass InFC)
        {
            ESRI.ArcGIS.Geodatabase.IGeoDataset pGeoDataSet;
            pGeoDataSet = InFC as IGeoDataset;
            ESRI.ArcGIS.Geometry.IEnvelope pEnvelope;
            pEnvelope = pGeoDataSet.Extent;
            int lngNumFeat;
            double dblArea;
            lngNumFeat = InFC.FeatureCount(null);
            if (lngNumFeat == 0 | pEnvelope.IsEmpty)
            {
                return 1000;
            }
            else
            {
                dblArea = pEnvelope.Height * pEnvelope.Width;
                return System.Math.Sqrt(dblArea / lngNumFeat);
            }
        }

        private static int Min(int v1, int v2)
        {
            return (v1 < v2 ? v1 : v2);
        }

        private static int Max(int v1, int v2)
        {
            return (v1 > v2 ? v1 : v2);
        }

        private static double Min(double v1, double v2)
        {
            return (v1 < v2 ? v1 : v2);
        }

        private static double Max(double v1, double v2)
        {
            return (v1 > v2 ? v1 : v2);
        }

        public static Dictionary<int, KeyValuePair<string, IEnumInvalidObject>> TransferMapDataWithinExtents(IMap map, IFeatureWorkspace outputWorkspace, IFeatureDatasetName outputFeatureDatasetName, ESRI.ArcGIS.ADF.Web.Geometry.Envelope downloadDataExtent, ISpatialReference outputSpatialReference)
        {
            Dictionary<int, KeyValuePair<string, IEnumInvalidObject>> invalidObjects = new Dictionary<int, KeyValuePair<string, IEnumInvalidObject>>();
            IWorkspaceName outWorkspaceName = GeodatabaseUtil.GetWorkspaceName((IWorkspace)outputWorkspace);

            List<IFeatureLayer> featureLayers = GetFeatureLayers(map);

            int counter = 0;
            foreach (IFeatureLayer featureLayer in featureLayers)
            {
                IFeatureClass inFeatureClass = featureLayer.FeatureClass;
                IDataset dataset = (IDataset)inFeatureClass;
                IFeatureWorkspace inFeatureWorkspace = (IFeatureWorkspace)dataset.Workspace;

                IEnvelope env = new ESRI.ArcGIS.Geometry.EnvelopeClass();
                env.XMin = downloadDataExtent.XMin;
                env.YMin = downloadDataExtent.YMin;
                env.XMax = downloadDataExtent.XMax;
                env.YMax = downloadDataExtent.YMax;

                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = env;
                spatialFilter.GeometryField = inFeatureClass.ShapeFieldName;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                IFeatureClassName outputFeatureClassName = GeodatabaseUtil.GetFeatureClassName(outWorkspaceName, dataset.Name);

                IEnumInvalidObject eInvalidObject = TransferData(inFeatureWorkspace, inFeatureClass, outputWorkspace, outputFeatureClassName, outputFeatureDatasetName, spatialFilter, outputSpatialReference);
                invalidObjects.Add(counter, new KeyValuePair<string, IEnumInvalidObject>(dataset.Name, eInvalidObject));
                counter++;
            }

            return invalidObjects;
        }

        /// <summary>
        /// Gets the workspace name reference for the PGDB file specified.
        /// </summary>
        /// <param name="pgdbFile">The PGDB file.</param>
        /// <returns>Workspace name reference</returns>
        public static IWorkspaceName GetPGDBWorkspaceName(string pgdbFile)
        {
            if(string.IsNullOrEmpty(pgdbFile))
                throw new ArgumentException("A valid PGDB file name [pgdbFile] is required.");

            IWorkspaceName workspaceName = new WorkspaceNameClass();
            workspaceName.WorkspaceFactoryProgID = "esriDataSourcesGDB.AccessWorkspaceFactory";
            workspaceName.PathName = pgdbFile;

            return workspaceName;
        }

        /// <summary>
        /// Gets the feature class name reference for the feature class specified in the workspace name reference.
        /// </summary>
        /// <param name="workspaceName">Workspace name reference.</param>
        /// <param name="featureclassName">Name of the featureclass.</param>
        /// <returns>FeatureClass name reference</returns>
        public static IFeatureClassName GetFeatureClassName(IWorkspaceName workspaceName, string featureclassName)
        {
            if (string.IsNullOrEmpty(featureclassName))
                throw new ArgumentException("A valid FeatureClass [featureclassName] name is required.");

            if (workspaceName == null)
                throw new ArgumentNullException("A [workspaceName] reference is required.");

            IFeatureClassName featureClassName = new FeatureClassNameClass();
            IDatasetName datasetName = (IDatasetName)featureClassName;
            datasetName.WorkspaceName = workspaceName;
            datasetName.Name = featureclassName;

            return featureClassName;
        }

        /// <summary>
        /// Gets the workspace name refernece of the workspace specified.
        /// </summary>
        /// <param name="workspace">The workspace reference.</param>
        /// <returns>workspace name reference</returns>
        public static IWorkspaceName GetWorkspaceName(IWorkspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException("A valid workspace reference [workspace] is required.");

            IDataset dataset = (IDataset)workspace;

            if (dataset == null)
                throw new ApplicationException("Cannot obtain workspace name from the workspace reference. The workspace argument passed in does not implement IDataset.");

            return (IWorkspaceName)dataset.FullName;
        }

        public static IMap GetMap(IMapDocument mapDocument, string dataframeName)
        {
            if (mapDocument == null)
                throw new ArgumentNullException("A valid map document [mapDocument] is required.");

            if (dataframeName == null)
                throw new ArgumentNullException("A valid dataframe name [dataframeName] is required.");

            IMap map = null;

            for (int i = 0; i < mapDocument.MapCount; i++)
            {
                if (string.Compare(mapDocument.get_Map(i).Name, dataframeName, true) == 0)
                {
                    map = mapDocument.get_Map(i);
                    break;
                }
            }
            return map;
        }

        public static List<ILayer> GetLayers(IMapDocument mapDocument, string dataframeName)
        {
            return (GetLayers(GetMap(mapDocument, dataframeName)));
        }

        public static List<IDataLayer> GetDataLayers(IMapDocument mapDocument, string dataframeName)
        {
            return (GetDataLayers(GetMap(mapDocument, dataframeName)));
        }

        public static List<IFeatureLayer> GetFeatureLayers(IMapDocument mapDocument, string dataframeName)
        {
            return (GetFeatureLayers(GetMap(mapDocument, dataframeName)));
        }

        public static List<IGeoFeatureLayer> GetGeoFeatureLayers(IMapDocument mapDocument, string dataframeName)
        {
            return (GetGeoFeatureLayers(GetMap(mapDocument, dataframeName)));
        }

        public static List<IGraphicsLayer> GetGraphicsLayers(IMapDocument mapDocument, string dataframeName)
        {
            return (GetGraphicsLayers(GetMap(mapDocument, dataframeName)));
        }

        public static List<IFDOGraphicsLayer> GetFDOGraphicsLayers(IMapDocument mapDocument, string dataframeName)
        {
            return (GetFDOGraphicsLayers(GetMap(mapDocument, dataframeName)));
        }

        public static List<ICoverageAnnotationLayer> GetCoverageAnnotationLayers(IMapDocument mapDocument, string dataframeName)
        {
            return (GetCoverageAnnotationLayers(GetMap(mapDocument, dataframeName)));
        }

        public static List<IGroupLayer> GetGroupLayers(IMapDocument mapDocument, string dataframeName)
        {
            return (GetGroupLayers(GetMap(mapDocument, dataframeName)));
        }

        public static List<ILayer> GetLayers(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException("A valid map reference [map] is required.");

            IEnumLayer elayers = map.get_Layers(null, true);
            List<ILayer> layers = new List<ILayer>();
            ILayer layer = elayers.Next();
            while (layer != null)
            {
                layers.Add(layer);
                layer = elayers.Next();
            }

            return layers;
        }

        public static List<IDataLayer> GetDataLayers(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException("A valid map reference [map] is required.");
            UID uid = new UIDClass();
            uid.Value = "{6CA416B1-E160-11D2-9F4E-00C04F6BC78E}";
            IEnumLayer elayers = map.get_Layers(uid, true);
            List<IDataLayer> layers = new List<IDataLayer>();
            ILayer layer = elayers.Next();
            while (layer != null)
            {
                if(layer is IDataLayer)
                    layers.Add((IDataLayer)layer);
                layer = elayers.Next();
            }

            return layers;
        }

        public static List<IFeatureLayer> GetFeatureLayers(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException("A valid map reference [map] is required.");
            UID uid = new UIDClass();
            uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";
            IEnumLayer elayers = map.get_Layers(uid, true);
            List<IFeatureLayer> layers = new List<IFeatureLayer>();
            ILayer layer = elayers.Next();
            while (layer != null)
            {
                if (layer is IFeatureLayer)
                    layers.Add((IFeatureLayer)layer);
                layer = elayers.Next();
            }

            return layers;
        }

        public static List<IGeoFeatureLayer> GetGeoFeatureLayers(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException("A valid map reference [map] is required.");
            UID uid = new UIDClass();
            uid.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
            IEnumLayer elayers = map.get_Layers(uid, true);
            List<IGeoFeatureLayer> layers = new List<IGeoFeatureLayer>();
            ILayer layer = elayers.Next();
            while (layer != null)
            {
                if (layer is IGeoFeatureLayer)
                    layers.Add((IGeoFeatureLayer)layer);
                layer = elayers.Next();
            }

            return layers;
        }

        public static List<IGraphicsLayer> GetGraphicsLayers(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException("A valid map reference [map] is required.");
            UID uid = new UIDClass();
            uid.Value = "{34B2EF81-F4AC-11D1-A245-080009B6F22B}";
            IEnumLayer elayers = map.get_Layers(uid, true);
            List<IGraphicsLayer> layers = new List<IGraphicsLayer>();
            ILayer layer = elayers.Next();
            while (layer != null)
            {
                if (layer is IGraphicsLayer)
                    layers.Add((IGraphicsLayer)layer);
                layer = elayers.Next();
            }

            return layers;
        }

        public static List<IFDOGraphicsLayer> GetFDOGraphicsLayers(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException("A valid map reference [map] is required.");
            UID uid = new UIDClass();
            uid.Value = "{5CEAE408-4C0A-437F-9DB3-054D83919850}";
            IEnumLayer elayers = map.get_Layers(uid, true);
            List<IFDOGraphicsLayer> layers = new List<IFDOGraphicsLayer>();
            ILayer layer = elayers.Next();
            while (layer != null)
            {
                if (layer is IFDOGraphicsLayer)
                    layers.Add((IFDOGraphicsLayer)layer);
                layer = elayers.Next();
            }

            return layers;
        }

        public static List<ICoverageAnnotationLayer> GetCoverageAnnotationLayers(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException("A valid map reference [map] is required.");
            UID uid = new UIDClass();
            uid.Value = "{0C22A4C7-DAFD-11D2-9F46-00C04F6BC78E}";
            IEnumLayer elayers = map.get_Layers(uid, true);
            List<ICoverageAnnotationLayer> layers = new List<ICoverageAnnotationLayer>();
            ILayer layer = elayers.Next();
            while (layer != null)
            {
                if (layer is ICoverageAnnotationLayer)
                    layers.Add((ICoverageAnnotationLayer)layer);
                layer = elayers.Next();
            }

            return layers;
        }

        public static List<IGroupLayer> GetGroupLayers(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException("A valid map reference [map] is required.");
            UID uid = new UIDClass();
            uid.Value = "{EDAD6644-1810-11D1-86AE-0000F8751720}";
            IEnumLayer elayers = map.get_Layers(uid, true);
            List<IGroupLayer> layers = new List<IGroupLayer>();
            ILayer layer = elayers.Next();
            while (layer != null)
            {
                if (layer is IGroupLayer)
                    layers.Add((IGroupLayer)layer);
                layer = elayers.Next();
            }

            return layers;
        }
    }
}
