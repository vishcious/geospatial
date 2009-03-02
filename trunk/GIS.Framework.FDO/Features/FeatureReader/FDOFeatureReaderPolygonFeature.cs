using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using OSGeo.FDO.Commands.Feature;

namespace GIS.Framework.FDO.Features.FeatureReader
{
    public class FDOFeatureReaderPolygonFeature : FDOFeatureReaderFeature
    {
        public FDOFeatureReaderPolygonFeature( IFeatureReader featureReader )
            : base( featureReader )
        {
        }

        public FDOFeatureReaderPolygonFeature( IFeatureReader featureReader, string geometryFieldName )
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
                    if( value is GeoAPI.Geometries.IPolygon )
                        FeatureShape = value as GeoAPI.Geometries.IPolygon;
                    else
                        throw new ArgumentException( "Shape should be of IPoint type.", "value" );
                }
            }
        }

        public GeoAPI.Geometries.IPolygon FeatureShape
        {
            get
            {
                return FDOFeatureReaderFeature.ConvertTo( FDOFeatureReaderFeature.GetGeometry( FeatureReader, GeometryFieldName ) ) as IPolygon;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
