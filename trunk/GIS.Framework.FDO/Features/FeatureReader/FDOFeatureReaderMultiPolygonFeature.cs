using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using OSGeo.FDO.Commands.Feature;

namespace GIS.Framework.FDO.Features.FeatureReader
{
    public class FDOFeatureReaderMultiPolygonFeature : FDOFeatureReaderFeature
    {
        public FDOFeatureReaderMultiPolygonFeature( IFeatureReader featureReader )
            : base( featureReader )
        {
        }

        public FDOFeatureReaderMultiPolygonFeature( IFeatureReader featureReader, string geometryFieldName )
            : base( featureReader, geometryFieldName )
        {
        }

        public override GeoAPI.Geometries.IGeometry Shape
        {
            get
            {
                return FeatureShape;
            }
            set
            {
                if( value == null )
                    FeatureShape = null;
                else
                {
                    if( value is GeoAPI.Geometries.IMultiPolygon )
                        FeatureShape = value as GeoAPI.Geometries.IMultiPolygon;
                    else
                        throw new ArgumentException( "Shape should be of IPoint type.", "value" );
                }
            }
        }

        public GeoAPI.Geometries.IMultiPolygon FeatureShape
        {
            get
            {
                return FDOFeatureReaderFeature.ConvertTo( FDOFeatureReaderFeature.GetGeometry( FeatureReader, GeometryFieldName ) ) as IMultiPolygon;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
