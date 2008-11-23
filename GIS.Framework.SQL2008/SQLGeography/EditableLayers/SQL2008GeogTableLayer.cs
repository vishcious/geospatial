using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using GIS.SQL2008;

namespace GIS.Framework.SQL2008.SQLGeography.Layers
{
    public abstract class SQL2008GeogTableLayer: SQL2008TableLayer, IGISEditableLayer
    {
        public SQL2008GeogTableLayer( SqlCommand dbCommand, string tableName, string shapeFieldName, string layerName, string keyFieldName )
            : base( dbCommand, tableName, shapeFieldName, layerName, keyFieldName )
        {
        }
        #region IGISEditableLayer Members

        public void Add( IGISFeature feature )
        {
            IDbCommand dbCommand = GetDBCommand();
            SqlCommand sqlCommand = SQLUtilities.CreateGeographyInsertCommand( GetTableName(), GetShapeFieldName(), GetGISFields().GetEnumerator(), feature );
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
