using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Server;

namespace GIS.SQL2008
{
    public interface ISQL2008AttributeTypeProvider
    {
        DataColumn GetAttributeType( Column column );
        Column GetAttributeType( DataColumn dataColumn );
    }
}
