using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GIS.Framework.Ao.Features
{
    public class AoMultiPointFeature : AoFeature
    {
        string _shapeField;

        public AoMultiPointFeature(IFeature feature, string shapeField)
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
                IMultipoint multipoint = GetShape() as IMultipoint;
                IPointCollection pointCollection = multipoint as IPointCollection;
                if (multipoint == null || multipoint.IsEmpty || pointCollection == null )
                    return null;
                else
                {
                    List<GeoAPI.Geometries.IPoint> list = new List<GeoAPI.Geometries.IPoint>();
                    for (int i = 0; i < pointCollection.PointCount; i++)
                    {
                        IPoint point = pointCollection.get_Point(i);
                        list.Add(new GisSharpBlog.NetTopologySuite.Geometries.Point(point.X, point.Y, point.Z));
                    }
                    GeoAPI.Geometries.IMultiPoint p = new GisSharpBlog.NetTopologySuite.Geometries.MultiPoint( list.ToArray() );
                    p.SRID = multipoint.SpatialReference.FactoryCode;
                    return p;
                }
            }
            set 
            {
                if( value == null )
                    SetShape(null);

                IMultipoint multipoint = new MultipointClass();
                IPointCollection pointCollection = multipoint as IPointCollection;
                foreach (GeoAPI.Geometries.IPoint point in value.Geometries)
                {
                    object Missing = Type.Missing;
                    IPoint newPoint = new PointClass();
                    newPoint.X = point.X;
                    newPoint.Y = point.Y;
                    newPoint.Z = point.Z;
                    pointCollection.AddPoint(newPoint, ref Missing, ref Missing);
                }
                multipoint.SpatialReference = ( ( ISpatialReferenceFactory3 ) new SpatialReferenceEnvironmentClass() ).CreateSpatialReference( value.SRID );
                SetShape(multipoint);
            }
        }
    }
}
