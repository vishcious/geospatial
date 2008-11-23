using System;
using System.Collections.Generic;
using System.Text;
using GeoAPI.Geometries;
using System.Xml;

namespace GIS.Framework
{
    /// <summary>
    /// This interface represents a GIS feature which is made up of two parts: The geometry and the attribute collection associated with the feature.
    /// </summary>
    public interface IGISFeature : IGISXMLSerializable, IGISJSONSerializable
    {
        /// <summary>
        /// Gets or sets the shape of the GIS feature.
        /// </summary>
        /// <value>The shape.</value>
        IGeometry Shape { get; set;}

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes associated with the GIS feature.</value>
        IGISAttributes Attributes { get; }
    }
}
