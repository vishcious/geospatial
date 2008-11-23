using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIS.Framework.Mem.Features;

namespace GIS.Framework.Mem.Layers
{
    public class MemMultiPointLayer: MemGISLayer
    {
        public MemMultiPointLayer(string layerName, string keyFieldName) : base(layerName, keyFieldName) { }

        public override GIS.Framework.Mem.Features.MemFeature CreateFeature()
        {
            return new MemMultiPointFeature();
        }
    }
}
