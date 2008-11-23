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
    public abstract class SQL2008EditableLayer: SQL2008Layer, IGISEditableLayer
    {
        string _tableName;
        SQL2008GISFields _gisFields;

        public SQL2008EditableLayer( SqlCommand dbCommand, string tableName, string shapeFieldName, string layerName, string keyFieldName )
            : base( dbCommand, shapeFieldName, layerName, keyFieldName )
        {
            if( string.IsNullOrEmpty( tableName ) )
                throw new ArgumentNullException( "tableName" );

            _tableName = tableName;
            dbCommand.CommandText = tableName;
            dbCommand.CommandType = CommandType.TableDirect;

            SqlDataReader dataReader = dbCommand.ExecuteReader( CommandBehavior.SchemaOnly );
            DataTable dataTable = dataReader.GetSchemaTable();
            _gisFields = new SQL2008GISFields( dataTable.Columns.ToArray() );
            dataReader.Close();
        }

        #region IGISEditableLayer Members

        public void Add( IGISFeature feature )
        {
            IDbCommand dbCommand = GetDBCommand();
            SqlCommand sqlCommand = SQLUtilities.CreateGeographyInsertCommand(_tableName, GetShapeFieldName(), _gisFields, feature);
            sqlCommand.Connection = dbCommand.Connection as SqlConnection;

            sqlCommand.ExecuteNonQuery();
        }

        public void Update( IGISFeature feature )
        {
            throw new NotImplementedException();
        }

        public void Delete( IGISFeature feature )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
