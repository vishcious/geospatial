using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESRI.ArcGIS.Geodatabase;

namespace GIS.ArcGIS
{
    public interface IAoAttributeTypeProvider
    {
        DataColumn GetAttributeType(IField2 field);
        IField2 GetAttributeType(DataColumn dataColumn);
    }
}
