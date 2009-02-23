using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.FDO.Schema;
using OSGeo.FDO.Commands.Feature;
using GIS.Common;
using OSGeo.FDO.Geometry;

namespace GIS.Framework.FDO.Features.FeatureReader
{
    public abstract class FDOFeatureReaderFeature: IGISFeature
    {
        IFeatureReader _featureReader;
        ClassDefinition _classDefinition;
        PropertyDefinitionCollection _propertyDefinitionCollection;
        PropertyDefinition _geometryPropertyDefinition;

        public FDOFeatureReaderFeature( IFeatureReader featureReader, string geometryFieldName )
        {
            featureReader.RequireArgument<IFeatureReader>( "featureReader" ).NotNull<IFeatureReader>();
            geometryFieldName.RequireArgument<string>( "geometryFieldName" ).NotNullOrEmpty();

            _featureReader = featureReader;
            _classDefinition = _featureReader.GetClassDefinition();
            _propertyDefinitionCollection = _classDefinition.Properties;
            
            IEnumerable<PropertyDefinition> temp = from PropertyDefinition pd in _propertyDefinitionCollection
                       where (pd.PropertyType == PropertyType.PropertyType_GeometricProperty && pd.Name.ToLower() == geometryFieldName.ToLower())
                       select pd;

            _geometryPropertyDefinition = temp.First<PropertyDefinition>();
            if( _geometryPropertyDefinition == null )
                throw new ArgumentException( string.Format("A geometry field with the given name '{0}' was not found", geometryFieldName) );
        }

        public FDOFeatureReaderFeature( IFeatureReader featureReader )
        {
            featureReader.RequireArgument<IFeatureReader>( "featureReader" ).NotNull<IFeatureReader>();

            _featureReader = featureReader;
            _classDefinition = _featureReader.GetClassDefinition();
            _propertyDefinitionCollection = _classDefinition.Properties;

            IEnumerable<PropertyDefinition> temp = from PropertyDefinition pd in _propertyDefinitionCollection
                                                   where pd.PropertyType == PropertyType.PropertyType_GeometricProperty
                                                   select pd;

            _geometryPropertyDefinition = temp.First<PropertyDefinition>();
            if( _geometryPropertyDefinition == null )
                throw new ArgumentException( "A geometry field was not found on the reader." );
        }

        #region IGISFeature Members

        public abstract GeoAPI.Geometries.IGeometry Shape
        {
            get;
            set;
        }

        public static GeoAPI.Geometries.IGeometry Convert( OSGeo.FDO.Geometry.IGeometry geometry )
        {
            GisSharpBlog.NetTopologySuite.IO.WKTReader wktReader = new GisSharpBlog.NetTopologySuite.IO.WKTReader();
            GeoAPI.Geometries.IGeometry ntsGeometry = wktReader.Read( geometry.Text );
            return ntsGeometry;
        }

        public static OSGeo.FDO.Geometry.IGeometry GetGeometry( IFeatureReader reader, string name )
        {
            byte[] geometryBytes = reader.GetGeometry( name );

            FgfGeometryFactory geometryFactory = new FgfGeometryFactory();
            IGeometry geometry = geometryFactory.CreateGeometryFromFgf( geometryBytes );
            return geometry;
        }

        public IGISAttributes Attributes
        {
            get
            {
                return new FDOFeatureReaderAttributes( _featureReader );
            }
        }

        #endregion

        #region IGISXMLSerializable Members

        public void ToXML( System.Xml.XmlWriter writer )
        {
            GeoXMLWriter.Write( this, writer );
        }

        #endregion

        #region IGISJSONSerializable Members

        public void ToJSON( Jayrock.Json.JsonTextWriter jwriter )
        {
            GeoJSONWriter.Write( this, jwriter );
        }

        #endregion
    }
}
