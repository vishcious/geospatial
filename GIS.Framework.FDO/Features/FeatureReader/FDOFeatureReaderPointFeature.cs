using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.FDO.Commands.Feature;
using GeoAPI.Geometries;

namespace GIS.Framework.FDO.Features.FeatureReader
{
    public class FDOFeatureReaderPointFeature: FDOFeatureReaderFeature
    {
        public FDOFeatureReaderPointFeature( IFeatureReader featureReader ) : base( featureReader )
        {
        }

        public FDOFeatureReaderPointFeature( IFeatureReader featureReader, string geometryFieldName ) : base( featureReader, geometryFieldName )
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
                    if( value is GeoAPI.Geometries.IPoint )
                        FeatureShape = value as GeoAPI.Geometries.IPoint;
                    else
                        throw new ArgumentException( "Shape should be of IPoint type.", "value" );
                }
            }
        }

        public IPoint FeatureShape
        {
            get
            {
                return FDOFeatureReaderFeature.ConvertTo( FDOFeatureReaderFeature.GetGeometry( FeatureReader, GeometryFieldName ) ) as IPoint;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
