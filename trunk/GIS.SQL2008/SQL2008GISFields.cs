using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GIS.Framework;
using GIS.SQL2008;
using Microsoft.SqlServer.Types;
using GeoAPI.Geometries;

namespace GIS.SQL2008
{
    public class SQL2008GISFields: IGISFields, IDisposable
    {
        string _geometryType = null;
        IList<DataColumn> _attributeFields = new List<DataColumn>();

        public SQL2008GISFields(IEnumerable<DataColumn> dataColumns)
        {
            if( dataColumns == null )
                throw new ArgumentNullException( "dataColumns" );

            foreach( DataColumn dataColumn in dataColumns )
            {
                if(dataColumn.DataType == typeof(SqlGeography) || dataColumn.DataType == typeof(SqlGeometry))
                {
                    if( _geometryType == null )
                        _geometryType = new DefaultGeometryFactory().GetGeometryType( typeof( IGeometryCollection ) );
                    else
                        _attributeFields.Add( dataColumn );
                }
                else
                {
                    _attributeFields.Add( dataColumn );
                }
            }

            if( _geometryType == null )
            {
                _attributeFields = null;
                throw new ArgumentException( "The geometry field is missing or the geometry type is not supported.", "fields" );
            }
        }


        #region IGISFields Members

        public string GeometryType
        {
            get
            {
                return _geometryType;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _attributeFields = null;
        }

        #endregion

        public int AttributeFieldCount
        {
            get
            {
                return _attributeFields.Count;
            }
        }

        #region IEnumerable<DataColumn> Members

        public IEnumerator<DataColumn> GetEnumerator()
        {
            return _attributeFields.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _attributeFields.GetEnumerator();
        }

        #endregion
    }
}
