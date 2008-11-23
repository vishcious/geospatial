using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.ComponentModel;

namespace GIS.Framework
{
    public class GeoXMLWriter
    {
        public static void Write(IGISAttributes attributes, XmlWriter xwriter)
        {
            if (attributes == null)
                throw new ArgumentNullException("attributes");
            if (xwriter == null)
                throw new ArgumentNullException("xwriter");

            xwriter.WriteStartElement("Attributes");

            IEnumerable<string> names = attributes.GetKeys();
            foreach (string name in names)
            {
                xwriter.WriteStartElement("item");
                //write the key
                xwriter.WriteElementString("key", name);
                //write the qualified type name of the value
                //write the value
                object value = attributes.GetValue(name);
                if (value == null)
                {
                    xwriter.WriteElementString("type", null);
                    xwriter.WriteElementString("value", null);
                }
                else
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(value.GetType());
                    xwriter.WriteElementString("type", value.GetType().AssemblyQualifiedName);
                    xwriter.WriteElementString("value", typeConverter.ConvertToString(value));
                }
                xwriter.WriteEndElement();
            }

            xwriter.WriteEndElement();
        }

        public static void Write(IGISAttributes attributes, TextWriter writer)
        {
            if (attributes == null)
                throw new ArgumentNullException("attributes");
            if (writer == null)
                throw new ArgumentNullException("writer");

            Write(attributes, XmlWriter.Create(writer));
        }

        public static void Write(IGISFeature feature, XmlWriter xwriter)
        {
            if (feature == null)
                throw new ArgumentNullException("feature");
            if (xwriter == null)
                throw new ArgumentNullException("xwriter");

            xwriter.WriteStartElement("Feature");

            if (feature.Shape != null)
                xwriter.WriteElementString("Geometry", feature.Shape.AsText());

            if (feature.Attributes != null)
                feature.Attributes.ToXML(xwriter);

            xwriter.WriteEndElement();
        }

        public static void Write(IGISFeature feature, TextWriter writer)
        {
            if (feature == null)
                throw new ArgumentNullException("feature");
            if (writer == null)
                throw new ArgumentNullException("writer");

            Write(feature, XmlWriter.Create(writer));
        }

        public static void Write(IGISLayer layer, XmlWriter xwriter)
        {
            if (layer == null)
                throw new ArgumentNullException("layer");
            if (xwriter == null)
                throw new ArgumentNullException("xwriter");

            xwriter.WriteStartElement("Layer");
            xwriter.WriteAttributeString("Name", layer.LayerName);
            xwriter.WriteAttributeString("KeyField", layer.KeyFieldName);

            while (layer.MoveNext())
            {
                layer.Current.ToXML(xwriter);
            }

            xwriter.WriteEndElement();
        }

        public static void Write(IGISLayer layer, TextWriter writer)
        {
            if (layer == null)
                throw new ArgumentNullException("layer");
            if (writer == null)
                throw new ArgumentNullException("writer");

            Write(layer, XmlWriter.Create(writer));
        }
    }
}
