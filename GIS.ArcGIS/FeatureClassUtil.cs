using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.GeoDatabaseDistributed;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;

namespace GIS.ArcGIS
{
    public static class FeatureClassUtil
    {

        public static ESRI.ArcGIS.Geodatabase.IWorkspace GetWorkspace(string inXMLFile)
        {
            IWorkspaceFactory inMemoryWorkspaceFactory = new InMemoryWorkspaceFactoryClass();
            IWorkspaceName workspaceName = inMemoryWorkspaceFactory.Create("", "InMemory", null, 0);
            IName name = workspaceName as IName;
            IWorkspace workspace = (IWorkspace)name.Open();

            //Create a GDB importer
            IGdbXmlImport xmlImport = new GdbImporterClass();

            // Read schema section from the XML file.
            // Note: enumNameMapping is initialized to nothing and its value is set 'by ref' with a call to the GenerateNameMapping method.
            IEnumNameMapping eNameMapping = null;
            xmlImport.GenerateNameMapping(inXMLFile, workspace, out eNameMapping);

            //Create the workspace schema and load the data
            xmlImport.ImportWorkspace(inXMLFile, eNameMapping, workspace, false);

            return workspace;

        }

		/// <summary>
		/// Opens the specified shapefile.
		/// </summary>
		/// <param name="shapefilePath">The full path to the shapefile.</param>
		/// <returns></returns>
		public static IFeatureClass OpenShapefile(string shapefilePath)
		{
			// Validate inputs
			if (string.IsNullOrEmpty(shapefilePath))
				throw new ArgumentNullException("shapefilePath", "Cannot open shapefile. Shapefile path has not been set.");
			if (System.IO.Path.GetExtension(shapefilePath.ToLower()) != ".shp")
				throw new ArgumentException("shapefilePath", string.Format("{0} is not a valid shapefile.", shapefilePath));

			// Get the path parts of the shapefile
			string workspaceDirectory = System.IO.Path.GetDirectoryName(shapefilePath);
			string featureClassName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

			// Open the shapefile and return the featureclass
			IWorkspaceFactory tempWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
			IFeatureWorkspace tempFeatureWorkspace = (IFeatureWorkspace)tempWorkspaceFactory.OpenFromFile(workspaceDirectory, 0);

			return tempFeatureWorkspace.OpenFeatureClass(featureClassName);
		}

        /// <summary>
        /// Checks if a FeatureClass with the specified name already exists in the workspace
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="featureClassName">Name of the feature class.</param>
        /// <param name="isNameFullyQualified">if set to <c>true</c> the 'featureClassName' argument passed in must be fully qualified else it should be just the name of the featureclass.</param>
        /// <returns>TRUE if a FeatureClass with the specified name exists in the workspace else FASLE</returns>
        public static bool DoesFeatureClassExist(IWorkspace workspace, string featureClassName, bool isNameFullyQualified)
        {
            IEnumDatasetName eDSNames = workspace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            IDatasetName DSName = eDSNames.Next();
            while (DSName != null)
            {
                if (!isNameFullyQualified)
                {
                    string[] nameParts = DSName.Name.Split('.');
                    if (nameParts[nameParts.Length - 1].ToLower() == featureClassName.ToLower())
                        return true;
               }
                else
                {
                    if (DSName.Name.ToLower() == featureClassName.ToLower())
                        return true;
                }
                DSName = eDSNames.Next();
            }
            return false;
        }

        /// <summary>
        /// Checks if a table with the specified name already exists in the workspace
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="isNameFullyQualified">if set to <c>true</c> the 'tableName' argument passed in must be fully qualified else it should be just the name of the table.</param>
        /// <returns>TRUE if a Table with the specified name exists in the workspace else FASLE</returns>
        public static bool DoesTableExist(IWorkspace workspace, string tableName, bool isNameFullyQualified)
        {
            IEnumDatasetName eDSNames = workspace.get_DatasetNames(esriDatasetType.esriDTTable);
            IDatasetName DSName = eDSNames.Next();
            while (DSName != null)
            {
                if (!isNameFullyQualified)
                {
                    string[] nameParts = DSName.Name.Split('.');
                    if (nameParts[nameParts.Length - 1].ToLower() == tableName.ToLower())
                        return true;
                }
                else
                {
                    if (DSName.Name.ToLower() == tableName.ToLower())
                        return true;
                }
                DSName = eDSNames.Next();
            }
            return false;
        }

        /// <summary>
        /// Opens the FeatureClass with the specified name from the specified workspace.
        /// </summary>
        /// <param name="Workspace">The workspace.</param>
        /// <param name="featureclassName">Fully qualified name of the featureclass.</param>
        /// <returns>Reference to FeatureClass if found else NULL</returns>
        public static IFeatureClass OpenFeatureClass(IFeatureWorkspace Workspace, string featureclassName)
        {
            // Check for invalid parameters.
            if (Workspace == null)
            {
                throw (new Exception("[Workspace] cannot be null"));
            }

            if (featureclassName == "")
            {
                throw (new Exception("[featureclassName] cannot be empty"));
            }

            // If Workspace is a IWorkspace2 then do the following
            if (!(Workspace is IWorkspace2))
            {
                throw new Exception("[Workspace] must implement IWorksapce2");
            }
            else
            {
                IWorkspace2 ws = Workspace as IWorkspace2;
                //If featureclass exists open it and return it else return null
                if (ws.get_NameExists(esriDatasetType.esriDTFeatureClass, featureclassName))
                {
                    return Workspace.OpenFeatureClass(featureclassName);
                }
                else
                {
                    return null; ;
                }
            }
        }

        /// <summary>
        /// Opens the FeatureClass with the specified name from the specified FeatureDataset.
        /// </summary>
        /// <param name="dataset">The feature dataset.</param>
        /// <param name="featureclassName">Fully qualifies name of the featureclass.</param>
        /// <returns>Reference to FeatureClass if found else NULL</returns>
        public static IFeatureClass OpenFeatureClass(IFeatureDataset dataset, string featureclassName)
        {
            // Check for invalid parameters.
            if (dataset == null)
            {
                throw (new Exception("[dataset] cannot be null"));
            }

            if (featureclassName == "")
            {
                throw (new Exception("[featureclassName] cannot be empty"));
            }

            // If Workspace is a IWorkspace2 then do the following
            if (!(dataset.Workspace is IFeatureWorkspace))
            {
                throw new Exception("Workspace containing [dataset] must implement IFeatureWorkspace");
            }
            else
            {
                if (!(dataset.Workspace is IWorkspace2))
                {
                    throw new Exception("Workspace containing [dataset] must implement IWorksapce2");
                }
                else
                {
                    IWorkspace2 ws = dataset.Workspace as IWorkspace2;
                    //If featureclass exists open it and return it else return null
                    if (ws.get_NameExists(esriDatasetType.esriDTFeatureClass, featureclassName))
                    {
                        IFeatureWorkspace workspace = dataset.Workspace as IFeatureWorkspace;
                        return workspace.OpenFeatureClass(featureclassName);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Opens the DBF table.
        /// </summary>
        /// <param name="dbfFileName">Name of the DBF file.</param>
        /// <returns></returns>
        public static ITable OpenDbfTable(string dbfFileName)
        {
            // Validate arguments
            if (string.IsNullOrEmpty(dbfFileName))
                throw new ArgumentNullException("dbfFileName", "DBF filename has not been set.");
            if (!System.IO.Path.GetExtension(dbfFileName.ToLower()).Equals(".dbf"))
                throw new ArgumentException("Input file must be DBF file format.", "dbfFileName");
            if (!File.Exists(dbfFileName))
                throw new FileNotFoundException(string.Format("{0} does not exist.", dbfFileName));

            // Get table name and directory
            string sourceTableName = System.IO.Path.GetFileNameWithoutExtension(dbfFileName);
            string sourceDirectory = System.IO.Path.GetDirectoryName(dbfFileName);

            // Open the shapefile (i.e. folder) workspace
            IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactoryClass();
            IWorkspace workspace = workspaceFactory.OpenFromFile(sourceDirectory, 0);

            // ShapefileWorkspace does not implement IWorkspace2. So manually check if table exists.
            if (!TableExists(workspace, sourceTableName))
                throw new ArgumentException(string.Format("{0} is an invalid table.", sourceTableName));

            // Open and return the table
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            return featureWorkspace.OpenTable(sourceTableName);
        }

        /// <summary>
        /// Creates the XY event source from a table which can be used to dynamically create a point feature class.
        /// </summary>
        /// <param name="xyTable">The table containing fields for X and Y coordinates.</param>
        /// <param name="xFieldName">Name of the x coordinate field.</param>
        /// <param name="yFieldName">Name of the y coordinate field.</param>
        /// <param name="spatialReference">The spatial reference of the points contained in the table.</param>
        /// <returns></returns>
        /// <remarks>An <see cref="IXYEventSource"/> is a dynmanic point feature class which generates features
        /// based on X and Y coordinates in a table.<para/>
        /// Since XYEventSource inherits from FeatureClass, it implements the same interfaces and can be treated like any other point feature class.</remarks>
        public static IXYEventSource CreateXYEventSourceFromTable(ITable xyTable, string xFieldName, string yFieldName, ISpatialReference spatialReference)
        {
            // Validate arguments
            if (xyTable == null)
                throw new ArgumentNullException("xyTable", "XY table has not been set.");
            if (string.IsNullOrEmpty(xFieldName))
                throw new ArgumentNullException("xFieldName", "X field name has not been set.");
            if (string.IsNullOrEmpty(yFieldName))
                throw new ArgumentNullException("yFieldName", "Y field name has not been set.");
            if (spatialReference == null)
                throw new ArgumentNullException("spatialReference", "Spatial reference has not been set.");

            // QI to get the dataset
            IDataset dataset = xyTable as IDataset;
            
            // Verify fields exist and are of a numeric type
            Int32 index = xyTable.FindField(xFieldName);
            if (index == -1)
                throw new ArgumentException(string.Format("{0} field does not exist in table {1}.", xFieldName, dataset.Name));
            index = xyTable.FindField(yFieldName);
            if (index == -1)
                throw new ArgumentException(string.Format("{0} field does not exist in table {1}.", yFieldName, dataset.Name));

            // Set the field properties for the XY event theme
            IXYEvent2FieldsProperties xyEvent2FieldsProperties = new XYEvent2FieldsPropertiesClass();
            xyEvent2FieldsProperties.XFieldName = xFieldName;
            xyEvent2FieldsProperties.YFieldName = yFieldName;
            xyEvent2FieldsProperties.ZFieldName = string.Empty;

            // Create XY event theme
            IXYEventSourceName xyEventSourceName = new XYEventSourceNameClass();
            xyEventSourceName.EventProperties = xyEvent2FieldsProperties;
            xyEventSourceName.SpatialReference = spatialReference;
            xyEventSourceName.EventTableName = dataset.FullName;

            // Create XY event source. 
            IName name = (xyEventSourceName as IName);
            IXYEventSource xyEventSource = (name.Open() as IXYEventSource);

            // Return value
            return xyEventSource;

        }

        /// <summary>
        /// Determines if a table exists within the specified workspace. <para/><b>Note</b> you should use <see cref="IWorkspace2.NameExists">IWorkspace2.NameExists</see>
        /// when working with workspaces other than a ShapefileWorkspace as it does not implement <see cref="IWorkspace2">IWorkspace2</see>.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static bool TableExists(IWorkspace workspace, string tableName)
        {
            // Loop over all table dataset names in workspace
            IEnumDatasetName datasetNames = workspace.get_DatasetNames(esriDatasetType.esriDTTable);
            if(datasetNames == null) return false;
            datasetNames.Reset();
            IDatasetName datasetName = datasetNames.Next();
            while (datasetName != null)
            {
                if (datasetName.Name.ToLower().Equals(tableName.ToLower()))
                {
                    return true;
                }
                datasetName = datasetNames.Next();
            }
            return false;
        }

        
    }
}
