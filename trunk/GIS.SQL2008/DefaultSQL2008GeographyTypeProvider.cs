using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Server;

namespace GIS.SQL2008
{
    public class DefaultSQL2008GeographyTypeProvider : ISQL2008GeometryTypeProvider
    {
        #region ISQL2008GeometryTypeProvider Members

        public string GetGeometryType( DataType geometryType )
        {
            if(DataType.Geography == geometryType)
                return "GeometryCollection";

            return null;
        }

        public DataType GetGeometryType( string geometryType )
        {
            if( string.Compare( geometryType, "Point", true ) == 0 )
            {
                return DataType.Geography;
            }
            else if( string.Compare( geometryType, "LineString", true ) == 0 )
            {
                return DataType.Geography;
            }
            else if( string.Compare( geometryType, "Polygon", true ) == 0 )
            {
                return DataType.Geography;
            }
            else if( string.Compare( geometryType, "MultiPolygon", true ) == 0 )
            {
                return DataType.Geography;
            }
            else if( string.Compare( geometryType, "MultiPoint", true ) == 0 )
            {
                return DataType.Geography;
            }
            else if( string.Compare( geometryType, "MultiLineString", true ) == 0 )
            {
                return DataType.Geography;
            }
            else if( string.Compare( geometryType, "GeometryCollection", true ) == 0 )
            {
                return DataType.Geography;
            }

            throw new NotSupportedException( "The geometry type '" + geometryType + "' is not supported." );
        }

        #endregion
    }
}
