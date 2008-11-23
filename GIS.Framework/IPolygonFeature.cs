using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;

namespace GIS.Framework
{
    public interface IPolygonFeature: IGISFeature
    {
        IPolygon FeatureShape { get; set; }
    }
}
