using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GIS.Framework
{
    public interface IGISXMLSerializable
    {
        /// <summary>
        /// Outputs the XML representation of the implmenting object.
        /// </summary>
        /// <param name="writer">The XML writer reference.</param>
        void ToXML(XmlWriter writer);
    }
}
