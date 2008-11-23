using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using Jayrock.Json;
using System.ComponentModel;

namespace GIS.Framework.Ao
{
    public class AoAttributes : IGISAttributes
    {
        IFeature _feature;
        string[] _nonSerializedFields;

        public AoAttributes(IFeature feature)
        {
            _feature = feature;
        }

        public AoAttributes(IFeature feature, string[] nonSerializedFields)
        {
            _feature = feature;
            _nonSerializedFields = nonSerializedFields;
        }

        public IFeature Feature
        {
            get { return _feature; }
        }

        public string[] NonSerializedFields
        {
            get { return _nonSerializedFields; }
            set { _nonSerializedFields = value; }
        }

        #region IGISAttributes Members

        public object GetValue(string attribute)
        {
            int index = _feature.Fields.FindField(attribute);
            if (index != -1)
            {
                return _feature.get_Value(index);
            }
            else
            {
                throw new IndexOutOfRangeException("Field name not found in the ArcObjects feature.");
            }
        }

        public void SetValue(string attribute, object value)
        {
            int index = _feature.Fields.FindField(attribute);
            if (index != -1)
            {
                _feature.set_Value(index, value);
            }
            else
            {
                throw new IndexOutOfRangeException("Field name not found in the ArcObjects feature.");
            }
        }

        public IEnumerable<string> GetKeys()
        {
            IFields fields = _feature.Fields;
            int count = fields.FieldCount;
            List<string> output = new List<string>();

            for (int i = 0; i < count; i++)
            {
                output.Add(fields.get_Field(i).Name);
            }
            return output;
        }

        #endregion

        #region IGISXMLSerializable Members

        public void ToXML(System.Xml.XmlWriter writer)
        {
            ToXMLWithOutType(writer);
        }

        private void ToXMLWithOutType(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Attributes");

            IEnumerable<string> keys = this.GetKeys();
            foreach (string item in keys)
            {
                if (NonSerializedFields != null && Contains(NonSerializedFields, item))
                    continue;
                writer.WriteStartElement("item");
                writer.WriteElementString("key", item.ToString());
                writer.WriteElementString("value", item.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private void ToXMLWithType(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Attributes");

            IEnumerable<string> keys = this.GetKeys();
            foreach (string item in keys)
            {
                if (NonSerializedFields != null && Contains(NonSerializedFields, item))
                    continue;

                writer.WriteStartElement("item");
                //write the key
                writer.WriteElementString("key", item);
                //write the qualified type name of the value
                //write the value
                object value = GetValue(item);
                if (value == null)
                {
                    writer.WriteElementString("type", null);
                    writer.WriteElementString("value", null);
                }
                else
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(value.GetType());
                    writer.WriteElementString("type", value.GetType().AssemblyQualifiedName);
                    writer.WriteElementString("value", typeConverter.ConvertToString(value));
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private bool Contains(string[] list, string value)
        {
            if (list == null)
                return false;
            foreach (string entry in list)
            {
                if (string.Compare(value, entry, false) == 0)
                    return true;
            }
            return false;
        }

        #endregion

        #region IGISJSONSerializable Members

        public void ToJSON(JsonTextWriter writer)
        {
            GeoJSONWriter.Write(this, writer, _nonSerializedFields);
        }

        #endregion
    }
}
