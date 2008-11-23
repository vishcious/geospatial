using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GIS.Framework
{
    public static class WKIDRanges
    {
        // Fields
        private static int[ , ] _gcsRanges = new int[ , ] { { 4000, 4999 }, { 37000, 37999 }, { 104000, 104999 }, { 204000, 204999 } };
        private static int[ , ] _pcsRanges = new int[ , ] { { 2000, 3999 }, { 20000, 32799 }, { 53000, 54999 }, { 65000, 65199 }, { 102000, 103999 }, { 202000, 203999 } };

        // Methods
        public static bool IsGeographic( int wkid )
        {
            for( int i = 0; i < ( _gcsRanges.Length / 2 ); i++ )
            {
                if( ( wkid >= _gcsRanges[ i, 0 ] ) && ( wkid <= _gcsRanges[ i, 1 ] ) )
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsProjected( int wkid )
        {
            for( int i = 0; i < ( _pcsRanges.Length / 2 ); i++ )
            {
                if( ( wkid >= _pcsRanges[ i, 0 ] ) && ( wkid <= _pcsRanges[ i, 1 ] ) )
                {
                    return true;
                }
            }
            return false;
        }

    }
}
