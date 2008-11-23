using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Jayrock.Json;

namespace GIS.Framework
{
    public interface IGISJSONSerializable
    {
        /// <summary>
        /// Outputs the JSON representation of the implementing object.
        /// </summary>
        /// <param name="jwriter">The jwriter.</param>
        void ToJSON(JsonTextWriter jwriter);
    }
}
