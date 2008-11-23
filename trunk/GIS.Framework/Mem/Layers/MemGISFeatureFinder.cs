using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIS.Framework.Mem.Features;

namespace GIS.Framework.Mem.Layers
{
    /// <summary>
    /// Helps check if a GIS feature matches a set of conditions.
    /// The conditions are represented in a list of key-value pairs where the KEY represents the field name to match
    /// and the VALUE represents the value to the matched in the corresponding field.
    /// </summary>
    public class MemGISFeatureFinder
    {
        IEnumerable<KeyValuePair<string, object>> _conditions;

        public MemGISFeatureFinder(IEnumerable<KeyValuePair<string, object>> conditions)
        {
            if (conditions == null)
                throw new ArgumentNullException("conditions");

            _conditions = conditions;
        }

        /// <summary>
        /// Determines whether the specified feature is match.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>
        /// 	<c>true</c> if the specified feature is match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(MemFeature feature)
        {
            int totalConditions = 0;
            int matchedConditions = 0;

            foreach (KeyValuePair<string, object> condition in _conditions)
            {
                totalConditions++;
                if (condition.Value == feature.Attributes.GetValue(condition.Key))
                {
                    matchedConditions++;
                    break;
                }
            }

            return (totalConditions == matchedConditions);
        }
    }
}
