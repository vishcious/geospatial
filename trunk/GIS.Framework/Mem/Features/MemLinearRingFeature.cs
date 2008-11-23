using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework.Mem.Features
{
    public class MemLinearRingFeature : MemFeature, ILinearRingFeature
    {
        ILinearRing _shape;

        public MemLinearRingFeature() { }

        public MemLinearRingFeature(Coordinate[] points)
        {
            Shape = new LinearRing(points);
        }

        public override IGeometry Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                if (value == null || value is ILinearRing)
                    _shape = value as ILinearRing;
                else
                {
                    if (value is ILineString)
                    {
                        ILineString line = value as ILineString;
                        if(line.Coordinates.Length == 0 ||line.IsClosed)
                            _shape = new LinearRing(line.Coordinates);
                    }
                    else
                        throw new ArgumentException("Shape should be of type [LinearRing]");
                }
            }
        }

        #region ILinearRingFeature members

        public ILinearRing FeatureShape
        {
            get { return Shape as ILinearRing; }
            set { Shape = value; }
        }

        #endregion
    }
}
