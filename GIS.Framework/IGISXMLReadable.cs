using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GIS.Framework
{
    public interface IGISXMLReadable
    {
        /// <summary>
        /// Populates the implementing object from the XML.
        /// </summary>
        /// <param name="reader">The XML reader reference.</param>
        void FromXML(XmlReader reader);
    }
}
