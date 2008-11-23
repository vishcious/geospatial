using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace GIS.Framework.SQL2008.SQLGeography.Layers
{
    public class LineStringLayer : SQL2008GeogTableLayer
    {
        public LineStringLayer( SqlCommand dbCommand, string tableName, string shapeFieldName, string layerName, string keyFieldName )
            : base( dbCommand, tableName, shapeFieldName, layerName, keyFieldName )
        {
        }

        public override IGISFeature CreateFeature(System.Data.IDataReader dataReader)
        {
            return new Features.SQL2008LineStringFeature(dataReader, GetShapeFieldName());
        }
    }
}
