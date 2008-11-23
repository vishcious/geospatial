using System;
using System.Collections.Generic;
using System.Text;

namespace GIS.Framework
{
    /// <summary>
    /// Represents a GIS layer that can be modified by adding new features, modifying existing features, deleting existing features.
    /// </summary>
    public interface IGISEditableLayer
    {
        /// <summary>
        /// Adds the specified feature the layer.
        /// </summary>
        /// <param name="feature">The feature to be added.</param>
        void Add(IGISFeature feature);
        /// <summary>
        /// Updates the layer with the specified feature.
        /// </summary>
        /// <param name="feature">The feature to update the layer with.</param>
        void Update(IGISFeature feature);
        /// <summary>
        /// Deletes the specified feature from the layer.
        /// </summary>
        /// <param name="feature">The feature to be deleted.</param>
        void Delete(IGISFeature feature);
    }
}
