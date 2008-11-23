using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using Jayrock.Json;
using ESRI.ArcGIS.Geometry;

namespace GIS.Framework.Ao.Features
{
    public abstract class AoFeature : GISFeature, ISupportsSRID
    {
        IFeature _feature;

        public AoFeature(IFeature feature)
            : base(new AoAttributes(feature))
        {
            if (feature == null)
                throw new ArgumentNullException("feature");

            _feature = feature;
        }

        public AoFeature(IFeature feature, string[] nonSerializedFields)
            : base(new AoAttributes(feature, nonSerializedFields))
        {
            if (feature == null)
                throw new ArgumentNullException("feature");

            _feature = feature;
        }

        protected IGeometry GetShape()
        {
            return _feature.Shape;
        }

        protected void SetShape(IGeometry geometry)
        {
            _feature.Shape = geometry;
        }

        #region ISupportsSRID Members

        public int? Srid
        {
            get
            {
                IGeometry geometry = GetShape();
                if( geometry != null )
                    return geometry.SpatialReference.FactoryCode;
                else
                    return null;
            }
            set
            {
                IGeometry geometry = GetShape();
                if( geometry != null )
                    geometry.SpatialReference = ( ( ISpatialReferenceFactory3 ) new SpatialReferenceEnvironmentClass() ).CreateSpatialReference( Srid ?? 0 );
                else
                    throw new NullReferenceException( "Unable to set the SRID for the Ao feature. The 'Shape' for the feature is NULL." );
            }
        }

        #endregion
    }
}
