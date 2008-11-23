using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using GeoAPI.Geometries;

namespace GIS.Framework.Ao.Layers
{
    public class AoPointFCLayer :AoFCLayer
    {
        public AoPointFCLayer(IFeatureClass featureClass, string layerName, string keyFieldName)
            :base(featureClass, layerName, keyFieldName)
        {
        }

        public override IGISFeature CreateFeature(ESRI.ArcGIS.Geodatabase.IFeature feature)
        {
            return new Ao.Features.AoPointFeature(feature, GetFeatureClass().ShapeFieldName);
        }
    }
}
