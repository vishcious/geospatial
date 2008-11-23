using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework.Mem.Features
{
    public class MemLineStringFeature : MemFeature, ILineStringFeature
    {
        ILineString _shape;

        public MemLineStringFeature() { }

        public MemLineStringFeature(ICoordinate[] points)
        {
            Shape = new LineString(points);
        }

        public override IGeometry Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                if (value == null || value is ILineString)
                    _shape = value as ILineString;
                else
                    throw new ArgumentException("Shape should be of type [ILineString]");
            }
        }

        #region ILineStringFeature members

        public ILineString FeatureShape
        {
            get { return Shape as ILineString; }
            set { Shape = value; }
        }

        #endregion
    }
}
