using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using System.Data;

namespace GIS.Framework.SQL2008.SQLGeography.Features
{
    public class SQL2008PointFeature : SQL2008Feature
    {
        public SQL2008PointFeature(IDataReader dataReader, string shapeFieldName)
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
                    if (value is GeoAPI.Geometries.IPoint)
                        FeatureShape = value as GeoAPI.Geometries.IPoint;
                    else
                        throw new ArgumentException("Shape should be of IPoint type.", "value");
                }
            }
        }

        public GeoAPI.Geometries.IPoint FeatureShape
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
                    return geometry as GeoAPI.Geometries.IPoint;
                }
            }
            set
            {
                SetShape( value );
            }
        }
    }
}
