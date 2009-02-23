using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.FDO.Commands.Feature;
using GIS.Common;
using OSGeo.FDO.Connections;
using OSGeo.FDO.Commands;
using OSGeo.FDO.Filter;

namespace GIS.Framework.FDO.Layers.FeatureReader
{
    public abstract class FDOFeatureReaderLayer: IGISLayer
    {
        string _layerName;
        string _keyFieldName;
        IConnection _connection;
        IFeatureReader _featureReader;
        ISelect _selectCommand;

        public FDOFeatureReaderLayer( IConnection connection, string featureClassName, featureReader, string layerName, string keyFieldName )
        {
            connection.RequireArgument<IConnection>( "connection" ).NotNull<IConnection>();
            layerName.RequireArgument<string>( "layerName" ).NotNullOrEmpty();
            keyFieldName.RequireArgument<string>( "keyFieldName" ).NotNullOrEmpty();

            _connection = connection;
            _selectCommand = ( ISelect ) _connection.CreateCommand( CommandType.CommandType_Select );
            _layerName = layerName;
            _keyFieldName = keyFieldName;
        }

        #region IGISLayer Members

        public string LayerName
        {
            get
            {
                return _layerName;
            }
        }

        public string KeyFieldName
        {
            get
            {
                return _keyFieldName;
            }
        }

        public void Search( object query )
        {
            if(!(query is Filter))
                throw new ArgumentException("Only queries of type 'OSGeo.FDO.Filter' are supported.");

            Filter filter = (Filter) query;
            _selectCommand.Filter = filter;
            _featureReader = _selectCommand.Execute();
        }

        #endregion

        public abstract void CreateFeature(IFeatureReader _featureReader);

        #region IGISXMLSerializable Members

        public void ToXML( System.Xml.XmlWriter writer )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGISJSONSerializable Members

        public void ToJSON( Jayrock.Json.JsonTextWriter jwriter )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerator<IGISFeature> Members

        public IGISFeature Current
        {
            get
            {
                if(_featureReader != null)
                    CreateFeature(_featureReader);
                return null;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _selectCommand = null;
            _featureReader = null;
            _connection = null;
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get
            {
                if(_featureReader != null)
                    CreateFeature(_featureReader);
                return null;
            }
        }

        public bool MoveNext()
        {
            return _featureReader.ReadNext();
        }

        public void Reset()
        {
            _featureReader = _selectCommand.Execute();
        }

        #endregion
    }
}
