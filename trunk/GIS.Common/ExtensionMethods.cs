using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GIS.Common
{
    public static class ExtensionMethods
    {
        public static IEnumerable<T> ForEach<T>( this IEnumerable<T> source, Action<T> act )
        {
            foreach( T element in source )
                act( element );
            return source;
        }

        public static PropertyValue<T> RequireArgument<T>( this T item, string argName )
        {
            return new PropertyValue<T>( item, argName );
        }

        public static PropertyValue<T> NotNull<T>( this PropertyValue<T> item ) where T : class
        {
            if( item.Value == null )
                throw new ArgumentNullException( item.ArgName );
            return item;
        }

        public static PropertyValue<string> NotNullOrEmpty( this PropertyValue<string> item )
        {
            if( string.IsNullOrEmpty(item.Value) )
                throw new ArgumentNullException( item.ArgName );
            return item;
        }

        public static PropertyValue<string> ShorterThan( this PropertyValue<string> item, int limit )
        {
            if( item.Value.Length >= limit )
                throw new ArgumentException( string.Format( "Parameter {0} must be shorter than {1} chars", item.ArgName, limit ) );
            return item;
        }

        public static PropertyValue<string> StartsWith( this PropertyValue<string> item, string pattern )
        {
            if( !item.Value.StartsWith( pattern ) )
                throw new ArgumentException( string.Format( "Parameter {0} must start with {1}", item.ArgName, pattern ) );
            return item;
        }
    }
}
