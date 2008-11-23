using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIS.Framework.Mem.Features;

namespace GIS.Framework.Mem.Layers
{
    public class MemPolygonLayer: MemGISLayer
    {
        public MemPolygonLayer(string layerName, string keyFieldName) : base(layerName, keyFieldName) { }

        public override MemFeature CreateFeature()
        {
            return new MemPolygonFeature();
        }
    }
}
