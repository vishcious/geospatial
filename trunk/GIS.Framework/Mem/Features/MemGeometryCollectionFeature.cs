using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework.Mem.Features
{
    public class MemGeometryCollectionFeature: MemFeature, IGeometryCollectionFeature
    {
        IGeometryCollection _shape;

        #region Constructors

        public MemGeometryCollectionFeature() { }

        public MemGeometryCollectionFeature(IGeometry[] geometries) 
        {
            _shape = new GeometryCollection(geometries);
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
                if (value == null || value is IGeometryCollection)
                    _shape = value as IGeometryCollection;
                else
                    throw new ArgumentException("Shape should be of type [IGeometryCollection]");
            }
        }

        #region IGeometryCollectionFeature Members

        public IGeometryCollection FeatureShape
        {
            get { return _shape as IGeometryCollection; }
            set { Shape = value; }
        }

        #endregion
    }
}
