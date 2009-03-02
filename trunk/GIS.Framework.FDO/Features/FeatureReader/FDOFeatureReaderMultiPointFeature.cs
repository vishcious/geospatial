using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using OSGeo.FDO.Commands.Feature;

namespace GIS.Framework.FDO.Features.FeatureReader
{
    public class FDOFeatureReaderMultiPointFeature : FDOFeatureReaderFeature
    {
        public FDOFeatureReaderMultiPointFeature( IFeatureReader featureReader ) : base( featureReader )
        {
        }

        public FDOFeatureReaderMultiPointFeature( IFeatureReader featureReader, string geometryFieldName )
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
                    if( value is GeoAPI.Geometries.IMultiPoint )
                        FeatureShape = value as GeoAPI.Geometries.IMultiPoint;
                    else
                        throw new ArgumentException( "Shape should be of IPoint type.", "value" );
                }
            }
        }

        public GeoAPI.Geometries.IMultiPoint FeatureShape
        {
            get
            {
                return FDOFeatureReaderFeature.ConvertTo( FDOFeatureReaderFeature.GetGeometry( FeatureReader, GeometryFieldName ) ) as IMultiPoint;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
