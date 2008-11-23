using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;

namespace GIS.Framework
{
    public interface IMultiLineStringFeature: IGISFeature
    {
        IMultiLineString FeatureShape { get; set; }
    }
}
