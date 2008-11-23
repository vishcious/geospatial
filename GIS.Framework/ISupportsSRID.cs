using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GIS.Framework
{
    public interface ISupportsSRID
    {
        /// <summary>
        /// Gets or sets the spatial reference coordinate system identifier.
        /// </summary>
        /// <value>The spatial reference coordinate system identifer.</value>
        int? Srid
        {
            get;
            set;
        }
    }
}
