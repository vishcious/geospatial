using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GIS.Framework.Ao.Features
{
    public class AoMultiPolygonFeature : AoFeature
    {
        string _shapeField;

        public AoMultiPolygonFeature(IFeature feature, string shapeField)
            : base(feature, new string[] {shapeField})
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
                IPolygon4 polygon = GetShape() as IPolygon4;
                if( polygon == null || polygon.IsEmpty )
                    return null;
                else
                {
                    GeoAPI.Geometries.IMultiPolygon p = ConvertTo( polygon );
                    p.SRID = polygon.SpatialReference.FactoryCode;
                    return p;
                }
            }
            set
            {
                if( value == null )
                    SetShape( null );

                IPolygon4 p = ConvertFrom( value );
                p.SpatialReference = ( ( ISpatialReferenceFactory3 ) new SpatialReferenceEnvironmentClass() ).CreateSpatialReference( value.SRID );
                SetShape(p);
            }
        }

        #region "ConvertTo"

        public static GeoAPI.Geometries.IMultiPolygon ConvertTo(ESRI.ArcGIS.Geometry.IPolygon4 polygon)
        {
            GisSharpBlog.NetTopologySuite.Geometries.MultiPolygon output = null;
            List<GeoAPI.Geometries.IPolygon> list = new List<GeoAPI.Geometries.IPolygon>();

            //IPolygon4.ExteriorRingBag should be used instead of IPolygon.QueryExteriorRings,
            //which does not work in .NET because of C-Style Arrays
            IGeometryBag exteriorRings = polygon.ExteriorRingBag;

            //For each exterior rings find the number of interior rings associated with it and print it
            IEnumGeometry exteriorRingsEnum = exteriorRings as IEnumGeometry;
            exteriorRingsEnum.Reset();
            IRing currentExteriorRing = exteriorRingsEnum.Next() as IRing;
            while (currentExteriorRing != null)
            {
                GeoAPI.Geometries.ILinearRing shell = ConvertTo(currentExteriorRing);
                List<GeoAPI.Geometries.ILinearRing> holes = null;

                //IPolygon4.get_InteriorRingBag should be used instead of IPolygon.QueryInteriorRings,
                //which does not work in .NET because of C-Style Arrays
                IGeometryBag interiorRings = polygon.get_InteriorRingBag(currentExteriorRing);
                IGeometryCollection interiorRingCollection = interiorRings as IGeometryCollection;
                if (interiorRingCollection != null && interiorRingCollection.GeometryCount > 0)
                {
                    holes = new List<GeoAPI.Geometries.ILinearRing>();
                    int interiorRingsTotal = interiorRingCollection.GeometryCount;
                    for (int interiorRingIndex = 0; interiorRingIndex < interiorRingsTotal; interiorRingIndex++)
                    {
                        IRing currentInteriorRing = interiorRingCollection.get_Geometry(interiorRingIndex) as IRing;
                        if (currentInteriorRing != null)
                        {
                            holes.Add(ConvertTo(currentInteriorRing));
                        }
                    }

                    ////Note we do nothing with the interiorRings, but you can use them the same way as the IGeometryBag exteriorRings
                    //IRing currentInteriorRing = exteriorRingsEnum.Next() as IRing;
                    //while (currentInteriorRing != null)
                    //{
                    //    holes.Add(ConvertTo(currentInteriorRing));
                    //    currentInteriorRing = exteriorRingsEnum.Next() as IRing;
                    //}
                }

                GeoAPI.Geometries.IPolygon entry;
                if (holes != null && holes.Count > 0)
                    entry = new GisSharpBlog.NetTopologySuite.Geometries.Polygon(shell, holes.ToArray());
                else
                    entry = new GisSharpBlog.NetTopologySuite.Geometries.Polygon(shell, null);

                list.Add(entry);
                currentExteriorRing = exteriorRingsEnum.Next() as IRing;
            }

            if (list != null && list.Count > 0)
            {
                output = new GisSharpBlog.NetTopologySuite.Geometries.MultiPolygon(list.ToArray());
            }

            return output;
        }

        public static GeoAPI.Geometries.ILinearRing ConvertTo(ESRI.ArcGIS.Geometry.IRing ring)
        {
            ESRI.ArcGIS.Geometry.IPoint fromToPoint = ring.FromPoint;
            List<GeoAPI.Geometries.ICoordinate> list = new List<GeoAPI.Geometries.ICoordinate>();
            list.Add(ConvertTo(fromToPoint));
            ISegmentCollection segmentCollection = ring as ISegmentCollection;
            if (segmentCollection != null)
            {
                int count = segmentCollection.SegmentCount;
                for (int i = 0; i < count; i++)
                {
                    ISegment segment = segmentCollection.get_Segment(i);
                    if (!(segment is ILine))
                        throw new ApplicationException("Curves are not supported.");
                    list.Add(ConvertTo(segment.ToPoint));
                }
            }
            else
                throw new ApplicationException("No segments found in the ring geometry.");

            list.Add(ConvertTo(fromToPoint));

            return new GisSharpBlog.NetTopologySuite.Geometries.LinearRing(list.ToArray());
        }

        public static GeoAPI.Geometries.ICoordinate ConvertTo(ESRI.ArcGIS.Geometry.IPoint point)
        {
            if (point == null)
                throw new ArgumentNullException("point", "A valid point reference is required.");

            return new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(point.X, point.Y, point.Z);
        }

        #endregion

        #region "ConvertFrom"

        public static ESRI.ArcGIS.Geometry.IPolygon4 ConvertFrom(GeoAPI.Geometries.IMultiPolygon polygon)
        {
            if(polygon == null)
                return null;

            return null;
        }

        #endregion
    }
}
