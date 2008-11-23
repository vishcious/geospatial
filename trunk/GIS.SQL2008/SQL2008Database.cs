using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using GIS.Framework;
using Microsoft.SqlServer.Types;
using GIS.SQL2008;

namespace GIS.SQL2008
{
    public class SQL2008Database: IDisposable
    {
        SQL2008Server _server;
        public string DatabaseName{get; set;}
        Database _database;
        IGeometryFactory _geometryFactory = new DefaultGeometryFactory();
        ISQL2008GeometryTypeProvider _geometryTypeProvider = new DefaultSQL2008GeographyTypeProvider();
        ISQL2008AttributeTypeProvider _attributeTypeProvider;

        public SQL2008Database( string connectionString )
        {
            if( string.IsNullOrEmpty( connectionString ) )
                throw new ArgumentNullException( "connectionString" );

            SqlConnection sqlConnection = new SqlConnection( connectionString );
            sqlConnection.Open();
            if( !( sqlConnection.State == ConnectionState.Open ) )
            {
                throw new ApplicationException( "Unable to open a connection to the sql server specified in the connection string." );
            }

            Initialize( new SQL2008Server( sqlConnection ), sqlConnection.Database );
        }

        public SQL2008Database( SQL2008Server server, string databaseName )
        {
            Initialize( server, databaseName );
        }

        private void Initialize( SQL2008Server server, string databaseName )
        {
            if( server == null )
                throw new ArgumentNullException( "server" );
            if( string.IsNullOrEmpty( databaseName ) )
                throw new ArgumentNullException( "databaseName" );

            _server = server;

            if( !_server.ContainsDatabase( databaseName ) )
                throw new ApplicationException( "The database '" + "' is not present in the database.");

            DatabaseName = databaseName;
            //_database = new Database( _server.Server, databaseName );
            _database = _server.Server.Databases[ DatabaseName ];
        }

        #region Public Properties

        public IGeometryFactory GeometryFactory
        {
            get
            {
                return _geometryFactory;
            }
            set
            {
                _geometryFactory = value;
            }
        }

        public ISQL2008GeometryTypeProvider GeometryTypeProvider
        {
            get
            {
                return _geometryTypeProvider;
            }
            set
            {
                _geometryTypeProvider = value;
            }
        }

        #endregion

        public bool ContainsTable( string tableName )
        {
            DataTable tables = _database.EnumObjects( DatabaseObjectTypes.Table );
            return tables.AsEnumerable().Any<DataRow>( new TableNameFinder()
            {
                TableName = tableName
            }.IsMatch );
            //var rows = from row in tables.AsEnumerable()
            //           where string.Compare( tableName, row[ "Name" ].ToString(), true ) == 0 
            //           select row;
            //return rows.Count<DataRow>() > 0;
        }

        public void CreateGeographyTable( string tableName, IEnumerator<DataColumn> columns, string geographyFieldName )
        {
            if( String.IsNullOrEmpty( geographyFieldName ) )
                throw new ArgumentNullException( "geographyFieldName" );

            SetGeographyType();
            DataColumn dc = new DataColumn( geographyFieldName, typeof( SqlGeography ) );
            CreateTable( tableName, columns, dc);
        }

        public void CreateGeometryTable( string tableName, IEnumerator<DataColumn> columns, string geometryFieldName )
        {
            if( String.IsNullOrEmpty( geometryFieldName ) )
                throw new ArgumentNullException( "geometryFieldName" );

            SetGeometryType();
            DataColumn dc = new DataColumn( geometryFieldName, typeof( SqlGeometry ) ) ;
            CreateTable( tableName, columns, dc);
        }

        public void CreateTable( string tableName, IEnumerator<DataColumn> columns )
        {
            if( String.IsNullOrEmpty( tableName ) )
                throw new ArgumentNullException( "tableName" );
            if( columns == null )
                throw new ArgumentNullException( "columns" );

            CreateTable( tableName, columns, null );
        }

        private void CreateTable( string tableName, IEnumerator<DataColumn> columns , DataColumn geometryColumn)
        {
            if( String.IsNullOrEmpty( tableName ) )
                throw new ArgumentNullException( "tableName" );
            if( columns == null )
                throw new ArgumentNullException( "columns" );

            Table table = new Table( _database, tableName );
            _attributeTypeProvider = new DefaultSQL2008AttributeTypeProvider(table);


            if( geometryColumn != null )
            {
                DataType dataType = _geometryTypeProvider.GetGeometryType( _geometryFactory.GetGeometryType( typeof( GeoAPI.Geometries.IGeometryCollection ) ) );
                table.Columns.Add( new Column( table, geometryColumn.ColumnName, dataType ) );
            }

            columns.Reset();
            while(columns.MoveNext())
            {
                DataColumn dataColumn = columns.Current;
                Column c = GetColumn( dataColumn );
                //if(c!= null)
                //    table.Columns.Add(c);
                table.Columns.Add( c );
            }

            table.Create();
        }

        public void DeleteTable( string tableName )
        {
            if( ContainsTable( tableName ) )
            {
                Table table = _database.Tables[ tableName ];
                table.Drop();
                return;
            }

            throw new InvalidArgumentException( "The table '" + tableName + "' is not present in the database." );
        }

        public void SetGeographyType()
        {
            _geometryTypeProvider = new DefaultSQL2008GeographyTypeProvider();
        }

        public void SetGeometryType()
        {
            _geometryTypeProvider = new DefaultSQL2008GeometryTypeProvider();
        }

        private Column GetColumn( DataColumn dataColumn )
        {
            return _attributeTypeProvider.GetAttributeType( dataColumn );
        }

        public void CreateGeographyTable( IGISFields gisFields , string tableName, string geometryFieldName)
        {
            if( string.IsNullOrEmpty( tableName ) )
                throw new ArgumentNullException( "tableName" );
            if( gisFields == null )
                throw new ArgumentNullException( "gisFields" );

            CreateGeographyTable( tableName, gisFields.GetEnumerator(), geometryFieldName);
        }

        public void CreateGeometryTable( IGISFields gisFields, string tableName, string geometryFieldName )
        {
            if( string.IsNullOrEmpty( tableName ) )
                throw new ArgumentNullException( "tableName" );
            if( gisFields == null )
                throw new ArgumentNullException( "gisFields" );

            CreateGeometryTable( tableName, gisFields.GetEnumerator(), geometryFieldName );
        }

        #region Inner class

        private class TableNameFinder
        {
            public string TableName
            {
                get;
                set;
            }

            public bool IsMatch( DataRow row )
            {
                return string.Compare( TableName, row[ "Name" ].ToString(), true ) == 0;
            }
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            if(_server != null)
                _server.Dispose();
            _server = null;
            _database = null;
        }

        #endregion
    }
}
