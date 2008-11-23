using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQL2008Import
{
    /// <summary>
    /// Gets arguments passed from console type applications
    /// Written by Xealcom.net
    /// </summary>
    public class CmdArgExtractor
    {
        /// <summary>
        /// An array of valid prefixes, e.g. "/a:", "/b:" when you expect a user to
        /// pass args in the format "/a:alpha /b:beta".
        /// </summary>
        private string[] _validPrefixes;

        /// <summary>
        /// Args separator, e.g. '/' if you expect a user to pass args
        ///  in the format "/a:alpha /b:beta".
        /// </summary>
        private char _argsSep;

        /// <summary>
        /// Args value separator, e.g. ':' if you expect a user to pass args
        ///  in the format "/a:alpha /b:beta".
        /// </summary>
        private char _argsValSep;

        /// <summary>
        /// Initializes a new instance of the  class.
        /// </summary>
        /// <param name="validPrefixes">The valid prefixes, e.g. in "/a:alpha /b:beta"
        /// validPrefixes will be "/a:" and "/b:"</param>
        /// <param name="argsSep">The args separator - "/" in the above example.</param>
        /// <param name="argsValSep">The args value separator, ":" in the above example.</param>
        public CmdArgExtractor( string[] validPrefixes, char argsSep, char argsValSep )
        {
            //TODO: Trhow an error if _validPrefixes not present
            if( validPrefixes.Length == 0 )
            {
                throw new Exception( "validPrefixes array has no values." );
            }

            if( argsValSep.ToString().Trim().Length == 0 )
            {
                throw new Exception( "argsValSep cannot be blank." );
            }

            if( argsSep.ToString().Trim().Length == 0 )
            {
                throw new Exception( "argsSep cannot be blank." );
            }

            this._validPrefixes = validPrefixes;
            this._argsSep = argsSep;
            this._argsValSep = argsValSep;
        }


        /// <summary>
        /// Initializes a new instance of the  class.
        /// </summary>
        /// <param name="argsSep">The args separator,e.g "/" in "/argA"</param>
        /// <remarks>Can be used it when you expect args in the following format: "/argA /argB"</remarks>
        public CmdArgExtractor( char argsSep )
        {
            if( argsSep.ToString().Trim() == null )
            {
                throw new Exception( "argsSep cannot be blank." );
            }
            this._argsSep = argsSep;
        }

        /// <summary>
        /// Validates the command arguments prefixes.
        /// </summary>
        /// <param name="args">An array of arguments passes at Command Prompt.</param>
        /// <returns>True is we have expected prefixes.</returns>
        public bool ValidArgsPrefixes( string[] args )
        {
            bool retVal = false;

            //TODO: Trhow an error if _validPrefixes not present
            if( this._validPrefixes.Length == 0 )
            {
                throw new Exception( "validPrefixes array has no values." );
            }


            for( int i = 0; i < this._validPrefixes.Length; i++ )
            {
                this._validPrefixes[ i ] = this._validPrefixes[ i ].ToLower();

                // If number of valid prefixes is greater then number of
                // args passed by user, then we've compared all args,
                if( i == args.Length )
                {
                    break;
                }

                for( int k = 0; k < args.Length; k++ )
                {
                    args[ k ] = args[ k ].ToLower();

                    if( args[ k ].Contains( this._validPrefixes[ i ] ) )
                    {
                        retVal = true;
                        break;
                    }
                    else
                    {
                        retVal = false;
                    }
                }

                if( retVal == true )
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            return retVal;
        }


        /// <summary>
        /// Gets the args values.
        /// </summary>
        /// <param name="args">The args passed from the command line.</param>
        /// <returns>An array of args parameters values.</returns>
        /// <remarks>If the args passed by user are '/u:jimmy /password:secret', then
        /// the return array will contain 'jimmy' and 'pasword'.
        /// </remarks>
        public string[] GetArgValues( string[] args )
        {
            string[] retArgs = new string[ args.Length ];
            string[] tmpArr;
            char[] separators = new char[] { this._argsSep, this._argsValSep };

            for( int i = 0; i < retArgs.Length; i++ )
            {
                tmpArr = args[ i ].Split( separators, StringSplitOptions.RemoveEmptyEntries );
                retArgs[ i ] = tmpArr[ 1 ];
            }

            return retArgs;
        }


        /// <summary>
        /// Gets the args values.
        /// </summary>
        /// <param name="args">Array containing args.</param>
        /// <returns>An array of args parameters values.</returns>
        /// <remarks>The diference between this method and GetArgValues is that it does not use
        /// args value separator, it deals with args like '/jimmy /secret'.
        /// The return array in this case will conatain 'jimmy' and 'pasword'.
        /// </remarks>
        public string[] GetArgValuesSimple( string[] args )
        {
            string[] retArgs = new string[ args.Length ];
            string[] tmpArr;
            char[] separator = new char[] { this._argsSep };

            for( int i = 0; i < retArgs.Length; i++ )
            {
                tmpArr = args[ i ].Split( separator, StringSplitOptions.RemoveEmptyEntries );
                retArgs[ i ] = tmpArr[ 0 ];
            }

            return retArgs;
        }


        /// <summary>
        /// Gets 2-dimentional array containing arg values and qualifiers.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>Two dimentional array</returns>
        /// <remarks> If the args are "/a:alpha /b:beta",
        /// then the return array's first dimension wil contain "a" and "b"
        /// and the second dimension will contain "alpha" and "beta".
        /// </remarks>
        public string[ , ] GetArgsTwoDimArray( string[] args )
        {
            int firstDim = 2;
            string[ , ] retArgs = new string[ firstDim, args.Length ];
            string[] tmpArr;

            char[] separators = new char[] { this._argsSep, this._argsValSep };

            for( int i = 0; i < args.Length; i++ )
            {
                tmpArr = args[ i ].Split( separators, StringSplitOptions.RemoveEmptyEntries );

                retArgs[ 0, i ] = tmpArr[ 0 ];
                retArgs[ 1, i ] = tmpArr[ 1 ];
            }
            return retArgs;
        }
    }//class
}
