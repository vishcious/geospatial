using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework.Mem.Features
{
    public class MemMultiPolygonFeature : MemFeature, IMultiPolygonFeature
    {
        IMultiPolygon _shape;

        public MemMultiPolygonFeature() { }

        #region Constructors

        public MemMultiPolygonFeature(IPolygon[] polygons)
        {
            Shape = new MultiPolygon(polygons);
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
                if (value == null || value is IMultiPolygon)
                    _shape = value as IMultiPolygon;
                else
                    throw new ArgumentException("Shape should be of type [IMultiPolygon]");
            }
        }

        #region IMultiPolygonFeature members

        public IMultiPolygon FeatureShape
        {
            get { return Shape as IMultiPolygon; }
            set { Shape = value; }
        }

        #endregion
    }
}
