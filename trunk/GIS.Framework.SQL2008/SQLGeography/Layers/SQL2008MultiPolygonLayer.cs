using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace GIS.Framework.SQL2008.SQLGeography.Layers
{
    public class SQL2008MultiPolygonLayer : SQL2008Layer
    {
        public SQL2008MultiPolygonLayer(IDbCommand dbComand, string shapeFieldName, string layerName, string keyFieldName)
            : base(dbComand, shapeFieldName, layerName, keyFieldName) { }

        public override IGISFeature CreateFeature(System.Data.IDataReader dataReader)
        {
            return new Features.SQL2008MultiPolygonFeature(dataReader, GetShapeFieldName());
        }
    }
}
