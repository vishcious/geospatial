using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework.Mem.Features
{
    public class MemMultiPointFeature: MemFeature, IMultiPointFeature
    {
        IMultiPoint _shape;

        #region "Constructors"

        public MemMultiPointFeature() { }

        public MemMultiPointFeature(IPoint[] points) 
        {
            _shape = new MultiPoint(points);
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
                if (value == null || value is IMultiPoint)
                    _shape = value as IMultiPoint;
                else
                    throw new ArgumentException("Shape should be of type [IMultiPoint]");
            }
        }

        #region IMultiPointFeature Members

        public IMultiPoint FeatureShape
        {
            get { return _shape as IMultiPoint; }
            set { Shape = value; }
        }

        #endregion
    }
}
