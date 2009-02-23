using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.FDO.Commands.Feature;
using GIS.Common;
using OSGeo.FDO.Schema;

namespace GIS.Framework.FDO.Features.FeatureReader
{
    public class FDOFeatureReaderAttributes: IGISAttributes
    {
        IFeatureReader _featureReader;
        IDictionary<string, PropertyDefinition> _fields = new Dictionary<string,PropertyDefinition>();
        IEnumerable<string> _fieldNames;

        public FDOFeatureReaderAttributes( IFeatureReader featureReader, IEnumerable<string> fieldNames, IDictionary<string, PropertyDefinition> fields )
        {
            featureReader.RequireArgument<IFeatureReader>( "featureReader" ).NotNull<IFeatureReader>();
            fieldNames.RequireArgument<IEnumerable<string>>( "fieldNames" ).NotNull<IEnumerable<string>>();
            fields.RequireArgument<IDictionary<string, PropertyDefinition>>( "fields" ).NotNull<IDictionary<string, PropertyDefinition>>();

            _featureReader = featureReader;
            _fieldNames = fieldNames;
            _fields = fields;
        }

        public FDOFeatureReaderAttributes(IFeatureReader featureReader)
        {
            featureReader.RequireArgument<IFeatureReader>( "featureReader" ).NotNull<IFeatureReader>();

            _featureReader = featureReader;
            ClassDefinition classDefinition = _featureReader.GetClassDefinition();
            PropertyDefinitionCollection propertyDefinitionCollection = classDefinition.Properties;

            List<string> fieldNames = new List<string>();
            foreach( PropertyDefinition pd in propertyDefinitionCollection )
            {
                if( pd.PropertyType == PropertyType.PropertyType_DataProperty )
                {
                    fieldNames.Add( pd.Name );
                    _fields.Add( pd.Name, pd );
                }
            }
            _fieldNames = fieldNames;
        }

        #region IGISAttributes Members

        public object GetValue( string attribute )
        {
            PropertyDefinition pd = _fields[ attribute ];
            if( pd == null )
                throw new IndexOutOfRangeException( string.Format( "The attribute field '{0}' was not found." ) );
            else
            {
                DataPropertyDefinition dpd = pd as DataPropertyDefinition;
                if( dpd == null )
                    throw new InvalidOperationException( string.Format( "The attribute field definition for {0} could not be converted to 'DataPropertyDefinition'.", attribute ) );
                else
                    return GetValue( _featureReader, dpd.Name, dpd.DataType );
            }
        }

        public void SetValue( string attribute, object value )
        {
            throw new NotImplementedException();
        }

        public object GetValue( IFeatureReader reader, string propertyName, DataType propertyDataType )
        {
            switch( propertyDataType )
            {
                case DataType.DataType_Int32:
                    return reader.GetInt32( propertyName );
                case DataType.DataType_Int16:
                    return reader.GetInt16( propertyName );
                case DataType.DataType_Int64:
                    return reader.GetInt64( propertyName );
                case DataType.DataType_Double:
                    return reader.GetDouble( propertyName );
                case DataType.DataType_Decimal:
                    return reader.GetDouble( propertyName );
                case DataType.DataType_DateTime:
                    return reader.GetDateTime( propertyName );
                case DataType.DataType_BLOB:
                    return reader.GetLOB( propertyName );
                case DataType.DataType_CLOB:
                    return reader.GetLOB( propertyName );
                case DataType.DataType_String:
                    return reader.GetString( propertyName );
                case DataType.DataType_Single:
                    return reader.GetSingle( propertyName );
                case DataType.DataType_Boolean:
                    return reader.GetBoolean( propertyName );
                case DataType.DataType_Byte:
                    return reader.GetByte( propertyName );
                default:
                    break;
            }

            throw new InvalidOperationException(string.Format("The datatype of the attribute field {0} is not supported", propertyName));
        }

        public IEnumerable<string> GetKeys()
        {
            return _fieldNames;
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
