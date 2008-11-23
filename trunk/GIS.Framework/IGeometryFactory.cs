using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;

namespace GIS.Framework
{
    public interface IGeometryFactory
    {
        string GetGeometryType( Type type );
        string GetGeometryType(IGeometry geometry);
        IGeometry CreateGeometry(string geometryType);
    }
}
