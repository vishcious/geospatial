using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using System.Data;

namespace GIS.Framework.SQL2008.SQLGeography.Features
{
    public class SQL2008MultiPolygonFeature : SQL2008Feature
    {
        public SQL2008MultiPolygonFeature(IDataReader dataReader, string shapeFieldName)
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
                    if (value is GeoAPI.Geometries.IMultiPolygon)
                        FeatureShape = value as GeoAPI.Geometries.IMultiPolygon;
                    else
                        throw new ArgumentException("Shape should be of IMultiPolygon type.", "value");
                }
            }
        }

        public GeoAPI.Geometries.IMultiPolygon FeatureShape
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
                    return geometry as GeoAPI.Geometries.IMultiPolygon;
                }
            }
            set
            {
                SetShape( value );
            }
        }
    }
}
