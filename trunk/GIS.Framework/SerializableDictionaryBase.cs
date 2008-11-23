using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Reflection;
using System.Resources;

namespace GIS.Framework
{
    public abstract class SerializableDictionaryBase : IXmlSerializable
    
    {
        const string NS = "http://www.edats.com/xml/serialization";

        public abstract IDictionary Dictionary{get;set;}

        public void WriteXml(XmlWriter w)
        {
            w.WriteStartElement("dictionary", NS);
            foreach (object key in Dictionary.Keys)
            {
                object value = Dictionary[key];
                w.WriteStartElement("item", NS);
                w.WriteElementString("key", NS, key.ToString());
                w.WriteElementString("value", NS, value.ToString());
                w.WriteEndElement();
            }
            w.WriteEndElement();
        }

        public void ReadXml(XmlReader r)
        {
            r.Read(); // move past container
            r.ReadStartElement("dictionary");
            while (r.NodeType != XmlNodeType.EndElement)
            {
                r.ReadStartElement("item", NS);
                string key = r.ReadElementString("key", NS);
                string value = r.ReadElementString("value", NS);
                r.ReadEndElement();
                r.MoveToContent();
                Dictionary.Add(key, value);
            }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return XmlSchema.Read(new StringReader(
                    "<xs:schema id='DictionarySchema' targetNamespace='http://www.edats.com/xml/serialization' elementFormDefault='qualified' xmlns='http://www.edats.com/xml/serialization' xmlns:mstns='http://www.edats.com/xml/serialization' xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:complexType name='DictionaryType'><xs:sequence><xs:element name='item' type='ItemType' maxOccurs='unbounded' /></xs:sequence></xs:complexType><xs:complexType name='ItemType'><xs:sequence><xs:element name='key' type='xs:string' /><xs:element name='value' type='xs:string' /></xs:sequence></xs:complexType><xs:element name='dictionary' type='mstns:DictionaryType'></xs:element></xs:schema>"), null);

        }

    }
}
