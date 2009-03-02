using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using OSGeo.FDO.Commands.Feature;

namespace GIS.Framework.FDO.Features.FeatureReader
{
    public class FDOFeatureReaderMultiLinestringFeature : FDOFeatureReaderFeature
    {
        public FDOFeatureReaderMultiLinestringFeature( IFeatureReader featureReader ) : base( featureReader )
        {
        }

        public FDOFeatureReaderMultiLinestringFeature( IFeatureReader featureReader, string geometryFieldName )
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
                    if( value is GeoAPI.Geometries.IMultiLineString )
                        FeatureShape = value as GeoAPI.Geometries.IMultiLineString;
                    else
                        throw new ArgumentException( "Shape should be of IPoint type.", "value" );
                }
            }
        }

        public GeoAPI.Geometries.IMultiLineString FeatureShape
        {
            get
            {
                return FDOFeatureReaderFeature.ConvertTo( FDOFeatureReaderFeature.GetGeometry( FeatureReader, GeometryFieldName ) ) as IMultiLineString;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
