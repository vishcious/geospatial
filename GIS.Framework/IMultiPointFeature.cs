using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;

namespace GIS.Framework
{
    public interface IMultiPointFeature: IGISFeature
    {
        IMultiPoint FeatureShape { get; set; }
    }
}
