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
    public class DefaultSQL2008AttributeTypeProvider: ISQL2008AttributeTypeProvider
    {
        SqlSmoObject _parent;

        public DefaultSQL2008AttributeTypeProvider(SqlSmoObject parent)
        {
            if( parent == null )
                throw new ArgumentNullException( "parent" );

            _parent = parent;
        }

        #region ISQL2008AttributeTypeProvider Members

        public System.Data.DataColumn GetAttributeType( Column column )
        {
            if(column == null)
                throw new ArgumentNullException("column");

            if( column.DataType.Name == "int" )
                return new DataColumn( column.Name, typeof( Int32 ) );
            else if( column.DataType.Name == "bigint" )
                return new DataColumn( column.Name, typeof( Int64 ) );
            else if( column.DataType.Name == "decimal" )
                return new DataColumn( column.Name, typeof( decimal ) );
            else if( column.DataType.Name == "numeric" )
                return new DataColumn( column.Name, typeof( decimal ) );
            else if( column.DataType.Name == "money" )
                return new DataColumn( column.Name, typeof( decimal ) );
            else if( column.DataType.Name == "smallint" )
                return new DataColumn( column.Name, typeof( Int16 ) );
            else if( column.DataType.Name == "image" )
                return new DataColumn( column.Name, typeof( System.Drawing.Image ) );
            else if( column.DataType.Name == "varbinary" )
                return new DataColumn( column.Name, typeof( byte[] ) );
            else if( column.DataType.Name == "varbinarymax" )
            {
                DataColumn dc = new DataColumn( column.Name, typeof( byte[] ) );
                dc.MaxLength = column.DataType.MaximumLength;
                return dc;
            }
            else if( column.DataType.Name == "text" ) 
            {
                DataColumn dc = new DataColumn( column.Name, typeof( string ) );
                dc.MaxLength = column.DataType.MaximumLength;
                return dc;
            }
            else if( column.DataType.Name == "nvarchar" )      
            {
                DataColumn dc = new DataColumn( column.Name, typeof( string ) );
                dc.MaxLength = column.DataType.MaximumLength;
                return dc;
            }
            else if( column.DataType.Name == "nvarcharmax" )
            {
                DataColumn dc = new DataColumn( column.Name, typeof( string ) );
                dc.MaxLength = column.DataType.MaximumLength;
                return dc;
            }
            else if( column.DataType.Name == "nchar" )
            {
                DataColumn dc = new DataColumn( column.Name, typeof( string ) );
                dc.MaxLength = column.DataType.MaximumLength;
                return dc;
            }
            else if( column.DataType.Name == "date" )
                return new DataColumn( column.Name, typeof( DateTime ) );
            else if( column.DataType.Name == "datetime" )
                return new DataColumn( column.Name, typeof( DateTime ) );
            else if( column.DataType.Name == "uniqueidentifier" )
                return new DataColumn( column.Name, typeof( Guid ) );
            else if( column.DataType.Name == "tinyint" )
                return new DataColumn( column.Name, typeof( Int16 ) );

            return null;
        }

        public Column GetAttributeType( DataColumn dataColumn )
        {
            if( dataColumn == null )
                throw new ArgumentNullException( "dataColumn" );

            if( dataColumn.DataType == typeof( Int16 ) )
            {
                return new Column(_parent, dataColumn.ColumnName, DataType.SmallInt);
            }
            else if( dataColumn.DataType == typeof( Single ) )
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.SmallInt );
            }
            else if( dataColumn.DataType == typeof( int ) )
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.Int );
            }
            else if( dataColumn.DataType == typeof( Int64 ) )
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.BigInt );
            }
            else if(dataColumn.DataType == typeof(DateTime))
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.DateTime );
            }
            else if( dataColumn.DataType == typeof( double ) )
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.Numeric( 2, 18 ) );
            }
            else if( dataColumn.DataType == typeof( decimal ) )
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.Numeric( 2, 18 ) );
            }
            else if( dataColumn.DataType == typeof( System.Drawing.Image ) )
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.Image );
            }
            else if( dataColumn.DataType == typeof( byte[] ) && dataColumn.MaxLength == -1)
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.VarBinaryMax );
            }
            else if( dataColumn.DataType == typeof( byte[] ) )
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.VarBinary( dataColumn.MaxLength ) );
            }
            else if( dataColumn.DataType == typeof( string ) && dataColumn.MaxLength == -1 )
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.NVarCharMax );
            }
            else if( dataColumn.DataType == typeof( byte[] ) )
            {
                return new Column( _parent, dataColumn.ColumnName, DataType.NVarChar( dataColumn.MaxLength ) );
            }


            return null;
        }

        #endregion
    }
}
