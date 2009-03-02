using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using OSGeo.FDO.Commands.Feature;

namespace GIS.Framework.FDO.Features.FeatureReader
{
    public class FDOFeatureReaderLinearRingFeature : FDOFeatureReaderFeature
    {
        public FDOFeatureReaderLinearRingFeature( IFeatureReader featureReader )
            : base( featureReader )
        {
        }

        public FDOFeatureReaderLinearRingFeature( IFeatureReader featureReader, string geometryFieldName )
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
                    if( value is GeoAPI.Geometries.ILinearRing )
                        FeatureShape = value as GeoAPI.Geometries.ILinearRing;
                    else
                        throw new ArgumentException( "Shape should be of IPoint type.", "value" );
                }
            }
        }

        public GeoAPI.Geometries.ILinearRing FeatureShape
        {
            get
            {
                return FDOFeatureReaderFeature.ConvertTo( FDOFeatureReaderFeature.GetGeometry( FeatureReader, GeometryFieldName ) ) as ILinearRing;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
