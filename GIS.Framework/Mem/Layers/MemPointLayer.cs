using System;
using System.Collections.Generic;
using System.Text;
using GIS.Framework.Mem.Features;

namespace GIS.Framework.Mem.Layers
{
    public class MemPointLayer: MemGISLayer
    {
        public MemPointLayer(string layerName, string keyFieldName) : base(layerName, keyFieldName) { }

        public override MemFeature CreateFeature()
        {
            return new MemPointFeature();
        }
    }
}
