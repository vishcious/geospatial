using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace GIS.Framework.SQL2008.SQLGeometry.Layers
{
    public class SQL2008MultiPointLayer : SQL2008Layer
    {
        public SQL2008MultiPointLayer(IDbCommand dbComand, string shapeFieldName, string layerName, string keyFieldName)
            : base(dbComand, shapeFieldName, layerName, keyFieldName) { }

        public override IGISFeature CreateFeature(System.Data.IDataReader dataReader)
        {
            return new Features.SQL2008MultiPointFeature(dataReader, GetShapeFieldName());
        }
    }
}
