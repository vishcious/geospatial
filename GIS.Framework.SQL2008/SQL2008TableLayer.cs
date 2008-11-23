using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIS.Framework;
using System.Data;
using System.Data.SqlClient;
using GIS.SQL2008;

namespace GIS.Framework.SQL2008
{
    public abstract class SQL2008TableLayer: SQL2008Layer, ISupportsGISFields
    {
        string _tableName;
        SQL2008GISFields _gisFields;

        public SQL2008TableLayer( SqlCommand dbCommand, string tableName, string shapeFieldName, string layerName, string keyFieldName )
            : base( dbCommand, shapeFieldName, layerName, keyFieldName )
        {
            if( string.IsNullOrEmpty( tableName ) )
                throw new ArgumentNullException( "tableName" );

            _tableName = tableName;
            dbCommand.CommandText = "Select * from " + tableName;

            SqlDataReader dataReader = dbCommand.ExecuteReader( CommandBehavior.SchemaOnly );
            DataTable dataTable = dataReader.GetSchemaTable();
            List<DataColumn> columns = new List<DataColumn>();
            foreach(DataRow row in dataTable.Rows)
            {
                DataColumn dc = new DataColumn(row["ColumnName"] as string, row["DataType"] as Type);
                dc.AllowDBNull = (bool)row["AllowDBNull"];
                dc.AutoIncrement = ( bool ) row[ "IsAutoIncrement" ];
                columns.Add(dc);
            }
            _gisFields = new SQL2008GISFields( columns );
            dataReader.Close();
        }

        public string GetTableName()
        {
            return _tableName;
        }

        #region ISupportsGISFields Members

        public IGISFields GetGISFields()
        {
            return _gisFields;
        }

        #endregion
    }
}
