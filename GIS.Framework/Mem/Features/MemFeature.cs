using System;
using System.Collections.Generic;
using System.Text;
using GisSharpBlog.NetTopologySuite.Geometries;
using GisSharpBlog.NetTopologySuite.IO;
using System.Xml;

namespace GIS.Framework.Mem.Features
{
    public abstract class MemFeature: GISFeature, IGISXMLReadable, IGISJSONReadable, ISupportsSRID
    {
        public MemFeature()
            : base(new GISSerializableDictionary()) { }


        #region IGISXMLReadable Members

        /// <summary>
        /// Populates the feature from the XML.
        /// </summary>
        /// <param name="reader">The XML reader reference.</param>
        public void FromXML(XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.IsStartElement("Feature"))
            {
                reader.ReadStartElement("Feature");

                //Read the geometry
                if (reader.IsStartElement("Geometry"))
                {
                    reader.ReadStartElement("Geometry");
                    string wkt = reader.ReadString();
                    WKTReader wktReader = new WKTReader();
                    Shape = wktReader.Read(wkt);
                    reader.ReadEndElement();//Read [Geometry] end
                }

                //Read the attributes
                if (Attributes != null)
                {
                    if (Attributes is GISSerializableDictionary)
                    {
                        GISSerializableDictionary att = Attributes as GISSerializableDictionary;
                        att.FromXML(reader);
                    }
                }
                else
                {
                    throw new NullReferenceException("Cannot read attribute values into feature. The [Attributes] property is NULL.");
                }

                reader.ReadEndElement();//Read [Feature] end
            }
        }

        #endregion

        #region IGISJSONReadable Members

        public void FromJSON(Jayrock.Json.JsonTextReader jreader)
        {
            GeoJSONReader.ReadGISFeature(this as IGISFeature, jreader);
        }

        #endregion

        #region ISupportsSRID Members

        public int? Srid
        {
            get
            {
                if( Shape != null )
                    return Shape.SRID;
                else
                    return null;
            }
            set
            {
                if( Shape != null )
                    Shape.SRID = value ?? 0;
                else
                    throw new NullReferenceException( "Unable to set the SRID for the feature. The 'Shape' for the feature is NULL." );
            }
        }

        #endregion
    }
}
