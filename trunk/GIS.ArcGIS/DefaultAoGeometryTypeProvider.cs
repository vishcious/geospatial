using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using GIS.Framework;
using GeoAPI.Geometries;

namespace GIS.ArcGIS
{
    public class DefaultAoGeometryTypeProvider : IAoGeometryTypeProvider
    {
        GIS.Framework.IGeometryFactory _geometryFactory = new GIS.Framework.DefaultGeometryFactory();

        public GIS.Framework.IGeometryFactory GeometryFactory
        {
            get
            {
                return _geometryFactory;
            }
            set
            {
                _geometryFactory = value;
            }
        }

        #region IAoGeometryTypeProvider Members

        public string GetGeometryType(esriGeometryType geometryType)
        {
            if (geometryType == esriGeometryType.esriGeometryPoint)
                return _geometryFactory.GetGeometryType(typeof(GeoAPI.Geometries.IPoint));
            else if (geometryType == esriGeometryType.esriGeometryPolygon)
                return _geometryFactory.GetGeometryType( typeof( GeoAPI.Geometries.IMultiPolygon ) );
            else if (geometryType == esriGeometryType.esriGeometryMultipoint)
                return _geometryFactory.GetGeometryType( typeof( GeoAPI.Geometries.IMultiPoint ) );
            else if (geometryType == esriGeometryType.esriGeometryPolyline)
                return _geometryFactory.GetGeometryType( typeof( GeoAPI.Geometries.IMultiLineString ) );
            else if( geometryType == esriGeometryType.esriGeometryBag )
                return _geometryFactory.GetGeometryType( typeof( GeoAPI.Geometries.IGeometryCollection ) );

            return null;
        }

        public esriGeometryType GetGeometryType(string geometryType)
        {
            if(string.Compare(geometryType, "Point", true) == 0)
            {
                return esriGeometryType.esriGeometryPoint;
            }
            else if( string.Compare( geometryType, "LineString", true ) == 0 )
            {
                return esriGeometryType.esriGeometryPolyline;
            }
            else if( string.Compare( geometryType, "Polygon", true ) == 0 )
            {
                return esriGeometryType.esriGeometryPolygon;
            }
            else if (string.Compare(geometryType, "MultiPolygon", true) == 0)
            {
                return esriGeometryType.esriGeometryPolygon;
            }
            else if (string.Compare(geometryType, "MultiPoint", true) == 0)
            {
                return esriGeometryType.esriGeometryMultipoint;
            }
            else if (string.Compare(geometryType, "MultiLineString", true) == 0)
            {
                return esriGeometryType.esriGeometryPolyline;
            }
            else if( string.Compare( geometryType, "GeometryCollection", true ) == 0 )
            {
                return esriGeometryType.esriGeometryBag;
            }

            throw new NotSupportedException("The geometry type '" + geometryType + "' is not supported.");
        }

        #endregion
    }
}
