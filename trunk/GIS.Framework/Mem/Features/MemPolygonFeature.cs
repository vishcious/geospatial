using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework.Mem.Features
{
    public class MemPolygonFeature : MemFeature, IPolygonFeature
    {
        IPolygon _shape;

        #region Constructors

        public MemPolygonFeature() { }

        public MemPolygonFeature(ILinearRing outerShell, ILinearRing[] innerHoles)
        {
            Shape = new Polygon(outerShell, innerHoles);
        }

        #endregion

        public override IGeometry Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                if (value == null || value is IPolygon)
                    _shape = value as IPolygon;
                else
                    throw new ArgumentException("Shape should be of type [IPolygon]");
            }
        }

        #region IPolygonFeature members

        public IPolygon FeatureShape
        {
            get { return Shape as IPolygon; }
            set { Shape = value; }
        }

        #endregion
    }
}
