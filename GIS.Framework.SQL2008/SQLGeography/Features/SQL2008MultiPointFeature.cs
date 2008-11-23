using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using System.Data;

namespace GIS.Framework.SQL2008.SQLGeography.Features
{
    public class SQL2008MultiPointFeature : SQL2008Feature
    {
        public SQL2008MultiPointFeature(IDataReader dataReader, string shapeFieldName)
            : base(dataReader, shapeFieldName) { }

        public override GeoAPI.Geometries.IGeometry Shape
        {
            get
            {
                return FeatureShape;
            }
            set
            {
                if (value == null)
                    FeatureShape = null;
                else
                {
                    if (value is GeoAPI.Geometries.IMultiPoint)
                        FeatureShape = value as GeoAPI.Geometries.IMultiPoint;
                    else
                        throw new ArgumentException("Shape should be of IMultiPoint type.", "value");
                }
            }
        }

        public GeoAPI.Geometries.IMultiPoint FeatureShape
        {
            get
            {
                SqlGeography geography = GetShape();
                if (geography == null)
                    return null;
                else
                {
                    GisSharpBlog.NetTopologySuite.IO.WKTReader wktReader = new GisSharpBlog.NetTopologySuite.IO.WKTReader();
                    GeoAPI.Geometries.IGeometry geometry = wktReader.Read(geography.ToString());
                    return geometry as GeoAPI.Geometries.IMultiPoint;
                }
            }
            set
            {
                SetShape( value );
            }
        }
    }
}
