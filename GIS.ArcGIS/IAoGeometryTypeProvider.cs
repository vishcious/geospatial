using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace GIS.ArcGIS
{
    public interface IAoGeometryTypeProvider
    {
        string GetGeometryType(esriGeometryType geometryType);
        esriGeometryType GetGeometryType(string geometryType);
    }
}
