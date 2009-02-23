using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GIS.Common
{
    public class PropertyValue<T>
    {
        public T Value
        {
            get;
            set;
        }
        public string ArgName
        {
            get;
            set;
        }
        public PropertyValue( T value, string argName )
        {
            Value = value;
            ArgName = argName;
        }
    }
}
