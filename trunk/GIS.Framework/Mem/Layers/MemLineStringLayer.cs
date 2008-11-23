using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIS.Framework.Mem.Features;

namespace GIS.Framework.Mem.Layers
{
    public class MemLineStringLayer: MemGISLayer
    {
        public MemLineStringLayer(string layerName, string keyFieldName) : base(layerName, keyFieldName) { }

        public override MemFeature CreateFeature()
        {
            return new MemLineStringFeature();
        }
    }
}
