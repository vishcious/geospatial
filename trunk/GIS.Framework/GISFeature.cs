using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using GeoAPI.Geometries;
using System.IO;
using GisSharpBlog.NetTopologySuite.IO;
using Jayrock.Json;

namespace GIS.Framework
{
    /// <summary>
    /// An abstract class providing some base implementations for GIS Features.
    /// </summary>
    public abstract class GISFeature : IGISFeature
    {
        IGISAttributes _attributes;

        public GISFeature(IGISAttributes attributes) 
        {
            _attributes = attributes;
        }

        #region IGISFeature Members

        /// <summary>
        /// Gets the attributes for this GIS feature.
        /// </summary>
        /// <value>The attribute collection reference.</value>
        public IGISAttributes Attributes
        {
            get
            {
                return _attributes;
            }
        }

        /// <summary>
        /// Gets or sets the geometry of the GIS feature.
        /// </summary>
        /// <value>The geometry reference.</value>
        public abstract IGeometry Shape{get;set;}

        #endregion

        #region IGISXMLSerializable Members

        /// <summary>
        /// Outputs the XML representation of the GIS feature.
        /// </summary>
        /// <param name="writer">The XML writer reference.</param>
        public void ToXML(XmlWriter writer)
        {
            writer.WriteStartElement("Feature");

            if (Shape != null)
                writer.WriteElementString("Geometry", Shape.AsText());

            if (Attributes != null)
                Attributes.ToXML(writer);

            writer.WriteEndElement();
        }

        ///// <summary>
        ///// Populates the feature from the XML.
        ///// </summary>
        ///// <param name="reader">The XML reader reference.</param>
        //public void FromXML(XmlReader reader)
        //{
        //    if (reader.IsStartElement("Feature"))
        //    {
        //        reader.ReadStartElement("Feature");

        //        //Read the geometry
        //        if (reader.IsStartElement("Geometry"))
        //        {
        //            reader.ReadStartElement("Geometry");
        //            string wkt = reader.ReadString();
        //            WKTReader wktReader = new WKTReader();
        //            Shape = wktReader.Read(wkt);
        //            reader.ReadEndElement();//Read [Geometry] end
        //        }

        //        //Read the attributes
        //        if (Attributes != null)
        //        {
        //            Attributes.FromXML(reader);
        //        }
        //        else
        //        {
        //            throw new NullReferenceException("Cannot read attribute values into feature. The [Attributes] property is NULL.");
        //        }

        //        reader.ReadEndElement();//Read [Feature] end
        //    }
        //}

        #endregion

        #region IGISJSONSerializable Members

        /// <summary>
        /// Outputs the JSON representation of the GIS feature.
        /// </summary>
        /// <param name="jwriter">The json writer reference.</param>
        public void ToJSON(JsonTextWriter jwriter)
        {
            GeoJSONWriter.Write(this, jwriter);
        }

        ///// <summary>
        ///// Populates the GIS layer from the JSON.
        ///// </summary>
        ///// <param name="jreader">The json reader reference.</param>
        //public void FromJSON(JsonTextReader jreader)
        //{
        //    GeoJSONReader.ReadGISFeature(this as IGISFeature, jreader);
        //}

        #endregion

    }
}
