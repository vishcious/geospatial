using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using OSGeo.FDO.Commands.Feature;

namespace GIS.Framework.FDO.Features.FeatureReader
{
    public class FDOFeatureReaderLinestringFeature : FDOFeatureReaderFeature
    {
        public FDOFeatureReaderLinestringFeature( IFeatureReader featureReader )
            : base( featureReader )
        {
        }

        public FDOFeatureReaderLinestringFeature( IFeatureReader featureReader, string geometryFieldName )
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
                    if( value is GeoAPI.Geometries.ILineString )
                        FeatureShape = value as GeoAPI.Geometries.ILineString;
                    else
                        throw new ArgumentException( "Shape should be of IPoint type.", "value" );
                }
            }
        }

        public GeoAPI.Geometries.ILineString FeatureShape
        {
            get
            {
                return FDOFeatureReaderFeature.ConvertTo( FDOFeatureReaderFeature.GetGeometry( FeatureReader, GeometryFieldName ) ) as ILineString;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
