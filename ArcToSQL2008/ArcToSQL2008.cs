using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.CatalogUI;
using System.Windows.Forms;
using System.ComponentModel;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.Geodatabase;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using log4net;
using log4net.Config;
using GIS.SQL2008;
using GIS.ArcGIS;
using GIS.Framework;
using GIS.Framework.Ao.Layers;
using System.Data.SqlClient;
using System.Configuration;
using Castle.Core.Resource;
using System.IO;
using GIS.Framework.Ao.Layers;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Core;

namespace ArcToSQL2008
{
    /// <summary>
    /// Summary description for ArcToSQL2008.
    /// </summary>
    [Guid( "973585c3-1299-41d6-99ce-7773a2572113" )]
    [ClassInterface( ClassInterfaceType.None )]
    [ProgId( "ArcToSQL2008.ArcToSQL2008" )]
    public sealed class ArcToSQL2008 : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible( false )]
        static void RegisterFunction( Type registerType )
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration( registerType );

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible( false )]
        static void UnregisterFunction( Type registerType )
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration( registerType );

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration( Type registerType )
        {
            string regKey = string.Format( "HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID );
            GxCommands.Register( regKey );

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration( Type registerType )
        {
            string regKey = string.Format( "HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID );
            GxCommands.Unregister( regKey );

        }

        #endregion
        #endregion

        WindsorContainer container;
        private static readonly log4net.ILog _log = LogManager.GetLogger( typeof( ArcToSQL2008 ) );
        private IApplication m_application;
        //Configuration m_config;

        public ArcToSQL2008()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "GeoSpatial.NET"; //localizable text
            base.m_caption = "ArcToSQL2008";  //localizable text
            base.m_message = "Export FeatureClass to SQL 2008 database.";  //localizable text 
            base.m_toolTip = "Export FeatureClass to SQL 2008 database";  //localizable text 
            base.m_name = "GeoSpatialNET_ArcToSQL2008";   //unique id, non-localizable (e.g. "MyCategory_ArcCatalogCommand")
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap( GetType(), bitmapResourceName );

                //string configFileName = Path.GetFileNameWithoutExtension( typeof( ArcToSQL2008 ).Assembly.Location ) + ".config";
                //string basePath = Path.GetDirectoryName( typeof( ArcToSQL2008 ).Assembly.Location );
                
                //Configure the log4net component using the configuration file
                //But I am prefering to configure the log4net component as opposed to reading it from a config file.
                //XmlConfigurator.Configure(new FileInfo( Path.GetFileName(typeof(ArcToSQL2008).Assembly.Location) + ".config"));

                //Configure log4net in code
                ConfigureLog4Net();

                //The components can also be read from a configuration file using the code below
                //But I am prefering to not use the config file and adding all components in code
                //container = new WindsorContainer( new XmlInterpreter( new FileResource( configFileName, basePath ) ) );

                //Load all the components into the windsor container in code
                container = new WindsorContainer();
                container.AddComponent<AoFCLayer, AoPointFCLayer>( "AoPoint" );
                container.AddComponent<AoFCLayer, AoMultiPointFCLayer>( "AoMultiPoint" );
                container.AddComponent<AoFCLayer, AoPolygonFCLayer>( "AoMultiPolygon" );
                container.AddComponent<AoFCLayer, AoPolylineFCLayer>( "AoMultiLineString" );

                container.AddComponent<GIS.Framework.SQL2008.SQLGeography.Layers.SQL2008GeogTableLayer, GIS.Framework.SQL2008.SQLGeography.Layers.PointLayer>( "Sql2008GeogPoint" );
                container.AddComponent<GIS.Framework.SQL2008.SQLGeography.Layers.SQL2008GeogTableLayer, GIS.Framework.SQL2008.SQLGeography.Layers.MultiPointLayer>( "Sql2008GeogMultiPoint" );
                container.AddComponent<GIS.Framework.SQL2008.SQLGeography.Layers.SQL2008GeogTableLayer, GIS.Framework.SQL2008.SQLGeography.Layers.MultiPolygonLayer>( "Sql2008GeogMultiPolygon" );
                container.AddComponent<GIS.Framework.SQL2008.SQLGeography.Layers.SQL2008GeogTableLayer, GIS.Framework.SQL2008.SQLGeography.Layers.MultiLineStringLayer>( "Sql2008GeogMultiLineString" );

                container.AddComponent<GIS.Framework.SQL2008.SQLGeometry.Layers.SQL2008GeomTableLayer, GIS.Framework.SQL2008.SQLGeometry.Layers.PointLayer>( "Sql2008GeomPoint" );
                container.AddComponent<GIS.Framework.SQL2008.SQLGeometry.Layers.SQL2008GeomTableLayer, GIS.Framework.SQL2008.SQLGeometry.Layers.MultiPointLayer>( "Sql2008GeomMultiPoint" );
                container.AddComponent<GIS.Framework.SQL2008.SQLGeometry.Layers.SQL2008GeomTableLayer, GIS.Framework.SQL2008.SQLGeometry.Layers.MultiPolygonLayer>( "Sql2008GeomMultiPolygon" );
                container.AddComponent<GIS.Framework.SQL2008.SQLGeometry.Layers.SQL2008GeomTableLayer, GIS.Framework.SQL2008.SQLGeometry.Layers.MultiLineStringLayer>( "Sql2008GeomMultiLineString" );
            }
            catch( Exception ex )
            {
                System.Diagnostics.Trace.WriteLine( ex.Message, "Invalid Bitmap" );
            }
        }

        public void ConfigureLog4Net()
        {
            Logger root;
            root = ( ( Hierarchy ) LogManager.GetRepository() ).Root;
            root.AddAppender( GetRollingFileAppender() );
            root.Level = Level.All;
            root.Repository.Configured = true;
        }

        public IAppender GetRollingFileAppender()
        {
            RollingFileAppender lAppender = new RollingFileAppender();
            lAppender.Name = "RollingFile";
            lAppender.File = Path.Combine( Path.GetDirectoryName( typeof( ArcToSQL2008 ).Assembly.Location ), "ArcToSQL2008.log");
            lAppender.AppendToFile = true;
            lAppender.RollingStyle = RollingFileAppender.RollingMode.Size;
            lAppender.MaxSizeRollBackups = 10;
            lAppender.MaximumFileSize = "100KB";
            lAppender.StaticLogFileName = true;
            lAppender.Layout = new log4net.Layout.PatternLayout( "%newline%newline%date %-5level - %message%newline" );
            lAppender.Threshold = log4net.Core.Level.All;
            lAppender.ActivateOptions();
            return lAppender;
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate( object hook )
        {
            if( hook == null )
                return;

            m_application = hook as IApplication;

            //Disable if it is not ArcCatalog
            if( hook is IGxApplication )
                base.m_enabled = true;
            else
                base.m_enabled = false;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            string connectionString = null;
            string tableName = "ArcToSQL2008";

            try
            {
                //m_config = ConfigurationManager.OpenExeConfiguration( typeof( ArcToSQL2008 ).Assembly.Location );
                //ConnectionStringSettings connStr = m_config.ConnectionStrings.ConnectionStrings[ "SQL2008" ];
                //connectionString = connStr.ConnectionString;
                SmoAdmin dialog = new SmoAdmin();
                if( dialog.ShowDialog() == DialogResult.OK )
                {
                    connectionString = dialog.ConnectionString;
                    tableName = dialog.TableName;
                }
                else
                {
                    return;
                }
                dialog.Close();
                dialog.Dispose();
            }
            catch( Exception ex )
            {
                MessageBox.Show( "Error openeing configuration file." + System.Environment.NewLine + ex.ToString() );
            }

            Import( connectionString, tableName );
        }

        public void Import(string connectionString, string tableName)
        {
            if( string.IsNullOrEmpty( connectionString ) )
            {
                MessageBox.Show( "Unable to obtain connection SQL 2008 information.", "Error", MessageBoxButtons.OK );
                return;
            }

            IGxApplication gxApp = m_application as IGxApplication;
            IGxView gxView = gxApp.View;
            if( gxApp.Selection.Count > 0 )
            {
                if( gxApp.Selection.Count == 1 )
                {
                    IGxObject gxObj = gxApp.SelectedObject;
                    object selObj = gxObj.InternalObjectName.Open();
                    if( selObj is IFeatureClass )
                    {
                        IFeatureClass featureClass = selObj as IFeatureClass;
                        //ConnectionStringSettings connStr = m_config.ConnectionStrings.ConnectionStrings[ "SQL2008" ];
                        SqlConnection sqlConnection = new SqlConnection( connectionString );
                        sqlConnection.Open();
                        SQL2008Database sqlDatabase = new SQL2008Database( connectionString );

                        DefaultAoGeometryTypeProvider aoGeometryTypeProvider = new DefaultAoGeometryTypeProvider();
                        string geometryType = aoGeometryTypeProvider.GetGeometryType( featureClass.ShapeType );

                        if( String.IsNullOrEmpty( geometryType ) )
                        {
                            MessageBox.Show( "The geometry type of the source FeatureClass is not supported.", "Error", MessageBoxButtons.OK );
                            return;
                        }

                        System.Collections.IDictionary parameters = new System.Collections.Hashtable();
                        parameters.Add( "featureClass", featureClass );
                        parameters.Add( "layerName", tableName );
                        parameters.Add( "keyFieldName", featureClass.OIDFieldName );
                        AoFCLayer shapeFileLayer = container.Resolve<AoFCLayer>( "Ao" + geometryType, parameters );

                        if( shapeFileLayer == null )
                        {
                            MessageBox.Show( "Unable to create the input framework layer for the FeatureClass. Please check the component configuration.", "Error", MessageBoxButtons.OK );
                            return;
                        }

                        if( sqlDatabase.ContainsTable( tableName ) )
                        {
                            if( MessageBox.Show( "Overwrite existing table?", "Overwrite", MessageBoxButtons.YesNo ) == DialogResult.Yes )
                            {
                                sqlDatabase.DeleteTable( tableName );
                            }
                            else
                            {
                                MessageBox.Show( "Exiting export operation", "Information", MessageBoxButtons.OK );
                                return;
                            }
                        }

                        ISupportsGISFields shapeFileFields = shapeFileLayer as ISupportsGISFields;
                        IGISFields inputFields = shapeFileFields.GetGISFields();

                        SqlCommand sqlCommand;

                        int? srid = null;
                        if( shapeFileLayer is ISupportsSRID )
                            srid = ( shapeFileLayer as ISupportsSRID ).Srid;

                        IGISLayer sql2008Layer = null;
                        //Create the dictionary with the parameters to create the corresponding SQL 2008 layer
                        System.Collections.IDictionary sqlParameters = new System.Collections.Hashtable();
                        sqlParameters.Add( "tableName", tableName );
                        sqlParameters.Add( "shapeFieldName", "SHAPE" );
                        sqlParameters.Add( "layerName", tableName );
                        sqlParameters.Add( "keyFieldName", featureClass.OIDFieldName );

                        if( srid.HasValue && WKIDRanges.IsGeographic( srid.Value ) )
                        {
                            //Create the geography table with the fields from the FeatureClass
                            sqlDatabase.CreateGeographyTable( inputFields, tableName, "SHAPE" );
                            sqlCommand = new SqlCommand( "Select * from " + tableName, sqlConnection );
                            sqlParameters.Add( "dbCommand", sqlCommand );
                            sql2008Layer = container.Resolve<IGISLayer>( "Sql2008Geog" + geometryType, sqlParameters );
                        }
                        else
                        {
                            //Create the geometry table with the fields from the FeatureClass
                            sqlDatabase.CreateGeometryTable( inputFields, tableName, "SHAPE" );
                            sqlCommand = new SqlCommand( "Select * from " + tableName, sqlConnection );
                            sqlParameters.Add( "dbCommand", sqlCommand );
                            sql2008Layer = container.Resolve<IGISLayer>( "Sql2008Geom" + geometryType, sqlParameters );
                        }

                        //Get the editable layer reference to the destination sql table layer
                        IGISEditableLayer editableLayer = sql2008Layer as IGISEditableLayer;

                        //Quit the application if a valid editable layer reference could not be obtained
                        if( editableLayer == null )
                        {
                            MessageBox.Show( "The destination is either invalid or does not support editing.", "Error", MessageBoxButtons.OK );
                            return;
                        }

                        shapeFileLayer.Search( null );
                        string keyFieldName = shapeFileLayer.KeyFieldName;
                        while( shapeFileLayer.MoveNext() )
                        {
                            try
                            {
                                editableLayer.Add( shapeFileLayer.Current );
                            }
                            catch( Exception ex )
                            {
                                string msg = ex.Message + " Feature ID:- " + shapeFileLayer.Current.Attributes.GetValue( keyFieldName ).ToString();
                                _log.Error( msg, ex );
                            }
                        }

                        MessageBox.Show( "Import completed.", "Information", MessageBoxButtons.OK );
                    }
                    else
                    {
                        MessageBox.Show( "Only FeatureClasses can be exported to SQL 2008", "Information", MessageBoxButtons.OK );
                    }
                }
                else
                {
                    MessageBox.Show( "Please only one FeatureClass to export to SQL 2008." );
                    return;
                }
            }
            else
            {
                MessageBox.Show( "Please select a FeatureClass to export to SQL 2008." );
                return;
            }
        }

        #endregion
    }
}
