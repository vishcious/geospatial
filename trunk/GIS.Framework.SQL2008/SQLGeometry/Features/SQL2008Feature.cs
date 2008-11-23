using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Types;

namespace GIS.Framework.SQL2008.SQLGeometry.Features
{
    public abstract class SQL2008Feature : GISFeature, ISupportsSRID
    {
        IDataReader _dataReader;
        string _shapeFieldName;

        public SQL2008Feature(IDataReader dataReader, string shapeFieldName)
            :base(new SQL2008Attributes(dataReader, new string[]{shapeFieldName}))
        {
            if (dataReader == null)
                throw new ArgumentNullException("dataReader");
            if (string.IsNullOrEmpty(shapeFieldName))
                throw new ArgumentNullException("shapeFieldName");

            _dataReader = dataReader;
            _shapeFieldName = shapeFieldName;
        }

        protected SqlGeometry GetShape()
        {
            return _dataReader[_shapeFieldName] as SqlGeometry;
        }

        protected void SetShape( GeoAPI.Geometries.IGeometry shape )
        {
            throw new InvalidOperationException( "This data source does not support setting property values including geometry." );
        }

        protected IDataReader GetDataReader()
        {
            return _dataReader;
        }

        #region ISupportsSRID Members

        public int? Srid
        {
            get
            {
                SqlGeometry geometry = GetShape();
                if( geometry == null || geometry.IsNull )
                    return null;
                else
                {
                    return geometry.STSrid.Value;
                }
            }
            set
            {
                SqlGeometry geometry = GetShape();
                if( geometry == null || geometry.IsNull )
                    throw new NullReferenceException( "Unable to set the SRID for the feature. The 'Shape' for the feature is NULL." );
                else
                    geometry.STSrid = new System.Data.SqlTypes.SqlInt32( value ?? 0 );
            }
        }

        #endregion
    }
}
