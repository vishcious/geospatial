using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;

namespace GIS.Framework
{
    public class GeometryCollectionFeature: GISFeature
    {
        IGeometryCollection _shape;

        public GeometryCollectionFeature(IGISAttributes attributes) : base(attributes) { }

        public override IGeometry Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                if (value == null || value is IGeometryCollection)
                    _shape = value as IGeometryCollection;
                else
                    throw new ArgumentException("Shape should be of type [IGeometryCollection]");
            }
        }

        public IGeometryCollection FeatureShape
        {
            get
            {
                return Shape as IGeometryCollection;
            }
            set
            {
                Shape = value;
            }
        }
    }
}
