using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework.Mem.Features
{
    public class MemMultiLineStringFeature: MemFeature, IMultiLineStringFeature
    {
        IMultiLineString _shape;

        #region Constructors

        public MemMultiLineStringFeature() { }

        public MemMultiLineStringFeature(ILineString[] lines) 
        {
            _shape = new MultiLineString(lines);
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
                if (value == null || value is IMultiLineString)
                    _shape = value as IMultiLineString;
                else
                    throw new ArgumentException("Shape should be of type [IMultiLineString]");
            }
        }

        #region IMultiLineStringFeature Members

        public IMultiLineString FeatureShape
        {
            get { return _shape as IMultiLineString; }
            set { Shape = value; }
        }

        #endregion
    }
}
