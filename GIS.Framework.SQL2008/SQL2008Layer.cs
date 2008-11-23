using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GIS.Framework.SQL2008
{
    public abstract class SQL2008Layer : IGISLayer
    {
        IDbCommand _dbCommand;
        IDataReader _dataReader;
        string _layerName;
        string _keyFieldName;
        string _shapeFieldName;
        string _query = null;

        public SQL2008Layer(IDbCommand dbComand, string shapeFieldName, string layerName, string keyFieldName)
        {
            if (dbComand == null)
                throw new ArgumentNullException("dbComand");
            if (dbComand.Connection == null)
                throw new ArgumentException("A valid database connection is required.");

            if (string.IsNullOrEmpty(layerName))
                throw new ArgumentNullException("layerName");
            if (string.IsNullOrEmpty(keyFieldName))
                throw new ArgumentNullException("keyFieldName");
            if (string.IsNullOrEmpty(shapeFieldName))
                throw new ArgumentNullException("shapeFieldName");

            _dbCommand = dbComand;
            _layerName = layerName;
            _keyFieldName = keyFieldName;
            _shapeFieldName = shapeFieldName;
        }

        protected IDbCommand GetDBCommand()
        {
            return _dbCommand;
        }

        public string GetShapeFieldName()
        {
            return _shapeFieldName;
        }

        #region IGISLayer Members

        public string LayerName
        {
            get { return _layerName; }
        }

        public string KeyFieldName
        {
            get { return _keyFieldName; }
        }

        public abstract IGISFeature CreateFeature(IDataReader dataReader);

        public IGISFeature Current
        {
            get
            {
                if (_dataReader != null && (!(_dataReader.IsClosed)))
                    return CreateFeature(_dataReader);

                return null;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                if (_dataReader != null && (!(_dataReader.IsClosed)))
                    return CreateFeature(_dataReader);

                return null;
            }
        }

        public bool MoveNext()
        {
            if (_dataReader == null || _dataReader.IsClosed)
                return false;
            else
                return _dataReader.Read();
        }

        /// <summary>
        /// Searches the layer using specified query and initializes the layer with the search results collection.
        /// The query should be represented as a string. The string will be applied as the CommandText of the IDbCommand.
        /// </summary>
        /// <param name="query">The query.</param>
        public void Search(object query)
        {
            if (query == null)
            {
                _query = null;
                _dataReader = _dbCommand.ExecuteReader();
            }
            else
            {
                if (!(query is string))
                    throw new ArgumentException("The query should be represented as a string which will be applied as the CommandText on the IDbCommand.", "query");

                _query = query as string;

                _dbCommand.CommandText = query.ToString();
                _dataReader = _dbCommand.ExecuteReader();
            }
        }

        #endregion

        #region IGISXMLSerializable Members

        public void ToXML(System.Xml.XmlWriter writer)
        {
            GeoXMLWriter.Write(this, writer);
        }

        #endregion

        #region IGISJSONSerializable Members

        public void ToJSON(Jayrock.Json.JsonTextWriter jwriter)
        {
            GeoJSONWriter.Write(this, jwriter);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _dataReader = null;
            _dbCommand = null;
        }

        #endregion

        #region IEnumerator Members


        public void Reset()
        {
            Search(_query);
        }

        #endregion
    }
}
