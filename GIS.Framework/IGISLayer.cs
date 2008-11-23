using System;
using System.Collections.Generic;
using System.Text;

namespace GIS.Framework
{
    /// <summary>
    /// This represents a collection of GIS features.
    /// </summary>
    public interface IGISLayer: IGISXMLSerializable, IGISJSONSerializable, IEnumerator<IGISFeature>
    {
        /// <summary>
        /// Gets the name of the layer or the collection of GIS features.
        /// </summary>
        /// <value>The name of the layer.</value>
        string LayerName { get;}

        /// <summary>
        /// Gets the name of the key field that contains the value which uniquely identifies every GIS feature in the layer.
        /// </summary>
        /// <value>The name of the key field.</value>
        string KeyFieldName { get;}

        /// <summary>
        /// Searches the layer using specified query and initializes the layer with the search results collection.
        /// Initially, the Current GIS feature would be NULL.
        /// A Search() should be performed to initialize the layer with the desired feature collection.
        /// MoveNext() should be called to move the cursor to the first GIS feature in the collection.
        /// </summary>
        /// <param name="query">The query.</param>
        void Search(object query);
    }
}
