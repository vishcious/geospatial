using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework.Mem.Features
{
    public class MemPointFeature : MemFeature, IPointFeature
    {
        IPoint _shape;

        #region "Constructors"

        public MemPointFeature()
        {
            Shape = new Point(0, 0);
        }

        public MemPointFeature(double x, double y)
        {
            Shape = new Point(x, y);
        }

        public MemPointFeature(double x, double y, double z)
        {
            Shape = new Point(x, y, z);
        }

        public MemPointFeature(Coordinate coord)
        {
            Shape = new Point(coord);
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
                if (value == null || value is IPoint)
                    _shape = value as IPoint;
                else
                    throw new ArgumentException("Shape should be of type [IPoint]");
            }
        }

        #region IGISPointFeature Members

        public IPoint FeatureShape
        {
            get { return _shape as IPoint; }
            set { Shape = value; }
        }

        #endregion

    }
}
