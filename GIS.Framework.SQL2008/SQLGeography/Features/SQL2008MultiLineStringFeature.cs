using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using System.Data;

namespace GIS.Framework.SQL2008.SQLGeography.Features
{
    public class SQL2008MultiLineStringFeature : SQL2008Feature
    {
        public SQL2008MultiLineStringFeature(IDataReader dataReader, string shapeFieldName)
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
                    if (value is GeoAPI.Geometries.IMultiLineString)
                        FeatureShape = value as GeoAPI.Geometries.IMultiLineString;
                    else
                        throw new ArgumentException("Shape should be of IMultiLineString type.", "value");
                }
            }
        }

        public GeoAPI.Geometries.IMultiLineString FeatureShape
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
                    return geometry as GeoAPI.Geometries.IMultiLineString;
                }
            }
            set
            {
                SetShape( value );
            }
        }
    }
}
