using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jayrock.Json;

namespace GIS.Framework
{
    public interface IGISJSONReadable
    {
        /// <summary>
        /// Populates the implementing object from the JSON.
        /// </summary>
        /// <param name="jreader">The jreader.</param>
        void FromJSON(JsonTextReader jreader);
    }
}
