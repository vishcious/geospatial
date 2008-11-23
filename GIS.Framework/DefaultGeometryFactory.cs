using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace GIS.Framework
{
    public class DefaultGeometryFactory: IGeometryFactory
    {
        #region IGeometryFactory Members

        public string GetGeometryType( Type type )
        {
            if( type == null )
                throw new ArgumentNullException( "type" );

            if(type.Name == typeof(IPoint).Name || type.GetInterface(typeof(IPoint).Name) != null)
                return "Point";
            else if( type.Name == typeof( ILineString ).Name || type.GetInterface( typeof( ILineString ).Name ) != null )
                return "LineString";
            else if( type.Name == typeof( IPolygon ).Name || type.GetInterface( typeof( IPolygon ).Name ) != null )
                return "Polygon";
            else if( type.Name == typeof( IMultiPoint ).Name || type.GetInterface( typeof( IMultiPoint ).Name ) != null )
                return "MultiPoint";
            else if( type.Name == typeof( IMultiLineString ).Name || type.GetInterface( typeof( IMultiLineString ).Name ) != null )
                return "MultiLineString";
            else if( type.Name == typeof( IMultiPolygon ).Name || type.GetInterface( typeof( IMultiPolygon ).Name ) != null )
                return "MultiPolygon";
            else if( type.Name == typeof( IGeometryCollection ).Name || type.GetInterface( typeof( IGeometryCollection ).Name ) != null )
                return "GeometryCollection";

            return null;
        }

        public string GetGeometryType(IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry");

            if( geometry is IPoint )
                return "Point";
            else if( geometry is IMultiPoint )
                return "MultiPoint";
            else if( geometry is IMultiLineString )
                return "MultiLineString";
            else if( geometry is IMultiPolygon )
                return "MultiPolygon";
            else if( geometry is ILineString )
                return "LineString";
            else if( geometry is IPolygon )
                return "Polygon";
            else if( geometry is IGeometryCollection )
                return "GeometryCollection";

            return null;
        }

        public IGeometry CreateGeometry(string geometryType)
        {
            if (string.Compare(geometryType, "Point", true) == 0)
            {
                return new Point(null, new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory());
            }
            else if (string.Compare(geometryType, "MultiPoint", true) == 0)
            {
                return new MultiPoint(null, new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory());
            }
            else if (string.Compare(geometryType, "MultiLineString", true) == 0)
            {
                return new MultiLineString(null, new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory());
            }
            else if (string.Compare(geometryType, "MultiPolygon", true) == 0)
            {
                return new MultiPolygon(null, new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory());
            }
            else if (string.Compare(geometryType, "LineString", true) == 0)
            {
                return new LineString(null, new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory());
            }
            else if (string.Compare(geometryType, "Polygon", true) == 0)
            {
                return new Polygon(null, new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory());
            }
            else if( string.Compare( geometryType, "GeometryCollection", true ) == 0 )
            {
                return new GeometryCollection( null, new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory() );
            }

            throw new NotSupportedException("The geometry type '" + geometryType + "' is not supported.");
        }

        #endregion
    }
}
