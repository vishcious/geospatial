using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using GeoAPI.Geometries;
using Microsoft.SqlServer.Types;
using GIS.Framework;

namespace GIS.SQL2008
{
    public static class SQLUtilities
    {
        public const string EmptyGeography = "NULL"; //"geography::STGeomFromText(NULL, 0)";
        public const string EmptyGeometry = "NULL"; //"geometry::STGeomFromText(NULL, 0)";
        private static Dictionary<System.Type, SqlDbType> dbTypeTable = new Dictionary<Type,SqlDbType>();

        static SQLUtilities()
        {
            dbTypeTable.Add( typeof( Boolean ), SqlDbType.Bit );
            dbTypeTable.Add( typeof( Int16 ), SqlDbType.SmallInt );
            dbTypeTable.Add( typeof( Int32 ), SqlDbType.Int );
            dbTypeTable.Add( typeof( Int64 ), SqlDbType.BigInt );
            dbTypeTable.Add( typeof( Double ), SqlDbType.Float );
            dbTypeTable.Add( typeof( Decimal ), SqlDbType.Decimal );
            dbTypeTable.Add( typeof( String ), SqlDbType.VarChar );
            dbTypeTable.Add( typeof( DateTime ), SqlDbType.DateTime );
            dbTypeTable.Add( typeof( Byte[] ), SqlDbType.VarBinary );
            dbTypeTable.Add( typeof( Guid ), SqlDbType.UniqueIdentifier );
        }

        public static string ToText( IGeometry geometry, int? srid )
        {
            return String.Format("('{0}', {1})", (geometry == null) ? "NULL" : geometry.ToString(), (srid ?? 0).ToString());
        }

        public static string SQLforGeography( IGeometry geography)
        {
            if( geography == null )
                return EmptyGeography;
            return String.Format( "geography::STGeomFromText{0}", ToText( geography, geography.SRID ) );
        }

        public static string SQLforGeometry( IGeometry geometry)
        {
            if( geometry == null )
                return EmptyGeometry;
            return String.Format( "geometry::STGeomFromText{0}", ToText( geometry, geometry.SRID ) );
        }

        public static string SQLforGeography( IGeometry geography, int? srid )
        {
            return String.Format( "geography::STGeomFromText{0}", ToText(geography, srid) );
        }

        public static string SQLforGeometry( IGeometry geometry, int? srid )
        {
            return String.Format( "geometry::STGeomFromTextt{0}", ToText( geometry, srid ) );
        }

        public static IList<DataColumn> ToArray( this DataColumnCollection columns )
        {
            List<DataColumn> list = new List<DataColumn>();

            foreach( DataColumn dc in columns )
            {
                list.Add( dc );
            }

            return list;
        }

        #region SQL Insert Command generation methods

        // Returns a string containing all the fields in the table
        public static string BuildAllFieldsSQL( IEnumerable<DataColumn> columns )
        {
            string sql = "";
            foreach( DataColumn column in columns )
            {
                if( sql.Length > 0 )
                    sql += ", ";
                sql += column.ColumnName;
            }
            return sql;
        }

        // Returns a SQL INSERT command. Assumes autoincrement is identity (optional)
        public static string BuildInsertSQL( string tableName, IEnumerator<DataColumn> columns, string shapeFieldName )
        {
            StringBuilder sql = new StringBuilder( "INSERT INTO " + tableName + " (" );
            StringBuilder values = new StringBuilder( "VALUES (" );
            bool bFirst = true;
            bool bIdentity = false;

            columns.Reset();
            while(columns.MoveNext())
            {
                DataColumn column = columns.Current;
                if( column.AutoIncrement )
                {
                    bIdentity = true;
                }
                else
                {
                    if( bFirst )
                        bFirst = false;
                    else
                    {
                        sql.Append( ", " );
                        values.Append( ", " );
                    }

                    sql.Append( column.ColumnName );
                    values.Append( "@" );
                    values.Append( column.ColumnName );
                }
            }

            if( !string.IsNullOrEmpty( shapeFieldName ) )
            {
                if( !bFirst )
                {
                    sql.Append( ", " );
                    values.Append( ", " );
                }

                sql.Append( shapeFieldName );
                values.Append( "@" + shapeFieldName );
            }

            sql.Append( ") " );
            sql.Append( values.ToString() );
            sql.Append( ")" );

            if( bIdentity )
            {
                sql.Append( "; SELECT CAST(scope_identity() AS [SCOPE_IDENTITY]" );
                sql.Append( ")" );
            }

            return sql.ToString();
            ;
        }

        // Creates a SqlParameter and adds it to the command
        public static void InsertParameter( SqlCommand command, string parameterName, string sourceColumn, object value, SqlDbType sqlDbType )
        {
            SqlParameter parameter = new SqlParameter( parameterName, value );

            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = parameterName;
            parameter.SourceColumn = sourceColumn;
            parameter.SqlDbType = sqlDbType;
            parameter.SourceVersion = DataRowVersion.Current;

            command.Parameters.Add( parameter );
        }

        // Creates a geography SqlParameter and adds it to the command
        public static void InsertGeographyParameter( SqlCommand command, string parameterName, string sourceColumn, SqlGeography value)
        {
            SqlParameter parameter = new SqlParameter( parameterName, value );

            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = parameterName;
            parameter.SourceColumn = sourceColumn;
            parameter.SqlDbType = SqlDbType.Udt;
            parameter.UdtTypeName = "GEOGRAPHY";
            parameter.SourceVersion = DataRowVersion.Current;

            command.Parameters.Add( parameter );
        }

        // Creates a geometry SqlParameter and adds it to the command
        public static void InsertGeometryParameter( SqlCommand command, string parameterName, string sourceColumn, SqlGeometry value)
        {
            SqlParameter parameter = new SqlParameter( parameterName, value );

            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = parameterName;
            parameter.SourceColumn = sourceColumn;
            parameter.SqlDbType = SqlDbType.Udt;
            parameter.UdtTypeName = "GEOMETRY";
            parameter.SourceVersion = DataRowVersion.Current;

            command.Parameters.Add( parameter );
        }

        public static SqlCommand CreateGeographyInsertCommand( string tableName, string shapeFieldName, IEnumerator<DataColumn> columns, IGISFeature feature )
        {
            if( string.IsNullOrEmpty( tableName ) )
                throw new ArgumentNullException( "tableName" );
            if( string.IsNullOrEmpty( shapeFieldName ) )
                throw new ArgumentNullException( "shapeFieldName" );
            if( columns == null )
                throw new ArgumentNullException( "columns" );
            if( feature == null )
                throw new ArgumentNullException( "feature" );

            SqlCommand cmd = CreateInsertCommand( tableName, shapeFieldName, columns, feature.Attributes );

            int? srid = null;
            if( feature is ISupportsSRID )
                srid = ( feature as ISupportsSRID ).Srid;

            //string wkt = SQLforGeography( feature.Shape );
            string wkt = feature.Shape == null ? "NULL" : feature.Shape.ToString();
            SqlGeography geography = null;
            if( feature.Shape != null )
            {
                geography = SqlGeography.Parse( feature.Shape.ToString() );
            }
            else
            {
                geography = SqlGeography.Parse( EmptyGeometry );
            }
            if( srid.HasValue )
                geography.STSrid = new System.Data.SqlTypes.SqlInt32( srid.Value );
            InsertGeographyParameter( cmd, "@" + shapeFieldName, shapeFieldName, geography );

            return cmd;
        }

        public static SqlCommand CreateGeometryInsertCommand( string tableName, string shapeFieldName, IEnumerator<DataColumn> columns, IGISFeature feature )
        {
            if( string.IsNullOrEmpty( tableName ) )
                throw new ArgumentNullException( "tableName" );
            if( string.IsNullOrEmpty( shapeFieldName ) )
                throw new ArgumentNullException( "shapeFieldName" );
            if( columns == null )
                throw new ArgumentNullException( "columns" );
            if( feature == null )
                throw new ArgumentNullException( "feature" );

            SqlCommand cmd = CreateInsertCommand( tableName, shapeFieldName, columns, feature.Attributes );

            int? srid = null;
            if( feature is ISupportsSRID )
                srid = ( feature as ISupportsSRID ).Srid;

            //string wkt = SQLforGeometry( feature.Shape );
            string wkt = feature.Shape == null ? "NULL" : feature.Shape.ToString();
            SqlGeometry geometry = null;
            if( feature.Shape != null )
            {
                geometry = SqlGeometry.Parse( wkt );
            }
            else
            {
                geometry = SqlGeometry.Parse( EmptyGeometry );
            }
            if( srid.HasValue )
                geometry.STSrid = new System.Data.SqlTypes.SqlInt32( srid.Value );
            InsertGeometryParameter( cmd, "@" + shapeFieldName, shapeFieldName, geometry );

            return cmd;
        }

        // Creates a SqlCommand for inserting a DataRow
        public static SqlCommand CreateInsertCommand( string tableName, string shapeFieldName, IEnumerator<DataColumn> columns, IGISAttributes feature )
        {
            if( string.IsNullOrEmpty( tableName ) )
                throw new ArgumentNullException( "tableName" );
            if( columns == null )
                throw new ArgumentNullException( "columns" );
            if( feature == null )
                throw new ArgumentNullException( "feature" );

            string sql = BuildInsertSQL( tableName, columns, shapeFieldName );
            SqlCommand command = new SqlCommand( sql );
            command.CommandType = System.Data.CommandType.Text;

            columns.Reset();
            while(columns.MoveNext())
            {
                DataColumn column = columns.Current;
                if( !column.AutoIncrement )
                {
                    string parameterName = "@" + column.ColumnName;
                    if( column.DataType == typeof( SqlGeography ) )
                    {
                        SqlGeography geog = feature.GetValue( column.ColumnName ) as SqlGeography;
                        InsertGeographyParameter( command, parameterName, column.ColumnName, geog );
                    }
                    else if( column.DataType == typeof( SqlGeometry ) )
                    {
                        SqlGeometry geom = feature.GetValue( column.ColumnName ) as SqlGeometry;
                        InsertGeometryParameter( command, parameterName, column.ColumnName, geom );
                    }
                    else
                    {
                        object val = feature.GetValue( column.ColumnName );
                        InsertParameter( command, parameterName, column.ColumnName, val, GetSqlDbType( column.DataType ) );
                    }
                }
            }

            return command;
        }

        public static SqlDbType GetSqlDbType( Type type )
        {
            if( dbTypeTable.ContainsKey( type ) )
                return dbTypeTable[ type ];

            if( type == typeof( SqlGeography ) )
                return SqlDbType.Udt;
            if( type == typeof( SqlGeometry ) )
                return SqlDbType.Udt;

            return SqlDbType.NVarChar;
        }

        #endregion

    }
}
