using System;
using System.Collections.Generic;
using System.Text;

namespace GIS.Framework
{
    /// <summary>
    /// This interface represents the attribute collection associated with the GIS feature.
    /// It provides access to the attribute values for the fields collection.
    /// </summary>
    public interface IGISAttributes : IGISXMLSerializable, IGISJSONSerializable
    {
        /// <summary>
        /// Gets the attribute value for the field with the name specified.
        /// </summary>
        /// <param name="attribute">The field name.</param>
        /// <returns>The attribute value for the field specified</returns>
        object GetValue(string attribute);
        /// <summary>
        /// Sets the attribute value for the field with the name specified.
        /// </summary>
        /// <param name="attribute">The field name.</param>
        /// <param name="value">The attribute value.</param>
        void SetValue(string attribute, object value);
        /// <summary>
        /// Gets the field names.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetKeys();
    }
}
