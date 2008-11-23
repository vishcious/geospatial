using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GIS.Framework.Ao.Features
{
    public class AoMultiPolylineFeature : AoFeature
    {
        string _shapeField;

        public AoMultiPolylineFeature(IFeature feature, string shapeField)
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
                IPolyline lines = GetShape() as IPolyline;
                if( lines == null || lines.IsEmpty )
                    return null;
                else
                {
                    GeoAPI.Geometries.IMultiLineString l = ConvertTo( lines );
                    l.SRID = lines.SpatialReference.FactoryCode;
                    return l;
                }
            }
            set 
            {
                if( value == null )
                    SetShape( null );

                IPolyline l = ConvertFrom( value );
                l.SpatialReference = ( ( ISpatialReferenceFactory3 ) new SpatialReferenceEnvironmentClass() ).CreateSpatialReference( value.SRID );
                SetShape(l);
            }
        }

        private IPolyline ConvertFrom(GeoAPI.Geometries.IMultiLineString lines)
        {
            if (lines == null)
                return null;

            IPolyline polyline = new PolylineClass();
            IGeometryCollection paths = lines as IGeometryCollection;

            foreach (GeoAPI.Geometries.ILineString line in lines.Geometries)
            {
                IPath path = new PathClass();
                IPointCollection pointCollection = path as IPointCollection;
                object Missing = Type.Missing;
                IPoint point = new PointClass();
                foreach (GeoAPI.Geometries.ICoordinate coordinate in line.Coordinates)
                {
                    point.X = coordinate.X;
                    point.Y = coordinate.Y;
                    point.Z = coordinate.Z;
                    pointCollection.AddPoint(point, ref Missing, ref Missing);
                }
                paths.AddGeometry(path, ref Missing, ref Missing);
            }
            return polyline;
        }

        private GeoAPI.Geometries.IMultiLineString ConvertTo(IPolyline lines)
        {
            IGeometryCollection paths = lines as IGeometryCollection;
            if (paths != null)
            {
                List<GeoAPI.Geometries.ILineString> newLines = new List<GeoAPI.Geometries.ILineString>();
                for (int i = 0; i < paths.GeometryCount; i++)
                {
                    IPath path = paths.get_Geometry(i) as IPath;
                    if (path != null)
                    {
                        List<GeoAPI.Geometries.ICoordinate> list = new List<GeoAPI.Geometries.ICoordinate>();
                        IPointCollection pointCollection = path as IPointCollection;
                        for (int j = 0; j < pointCollection.PointCount; j++)
                        {
                            IPoint point = pointCollection.get_Point(j);
                            list.Add(new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(point.X, point.Y, point.Z));
                        }
                        newLines.Add(new GisSharpBlog.NetTopologySuite.Geometries.LineString(list.ToArray()));
                    }
                }
                return new GisSharpBlog.NetTopologySuite.Geometries.MultiLineString(newLines.ToArray());
            }
            
            return null;
        }
    }
}
