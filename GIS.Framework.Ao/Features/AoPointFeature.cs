using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework.Ao.Features
{
    public class AoPointFeature : AoFeature
    {
        string _shapeField;

        public AoPointFeature(IFeature feature, string shapeField)
            : base(feature, new string[] { shapeField }) 
        {
            _shapeField = shapeField;
        }

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
                IPoint point = GetShape() as IPoint;
                if( point == null || point.IsEmpty )
                    return null;
                else
                {
                    GisSharpBlog.NetTopologySuite.Geometries.Point p = new GisSharpBlog.NetTopologySuite.Geometries.Point( point.X, point.Y, point.Z );
                    p.SRID = point.SpatialReference.FactoryCode;
                    return p;
                }
            }
            set 
            {
                if( value == null )
                    SetShape( null );

                IPoint point = new PointClass();
                point.X = value.X;
                point.Y = value.Y;
                point.Z = value.Z;
                point.SpatialReference = ( ( ISpatialReferenceFactory3 ) new SpatialReferenceEnvironmentClass() ).CreateSpatialReference(value.SRID);
                SetShape(point);
            }
        }
    }
}
