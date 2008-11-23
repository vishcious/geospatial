using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jayrock.Json;
using System.ComponentModel;
using System.Data;

namespace GIS.Framework.SQL2008
{
    public class SQL2008Attributes : IGISAttributes
    {
        IDataReader _dataReader;
        string[] _nonSerializedFields;
        ICollection<string> _fields;

        public SQL2008Attributes(IDataReader dataReader)
        {
            if (dataReader == null)
                throw new ArgumentNullException("dataReader");

            _dataReader = dataReader;
            _fields = GetFields(_dataReader);
        }

        public SQL2008Attributes(IDataReader dataReader, string[] nonSerializedFields)
        {
            if (dataReader == null)
                throw new ArgumentNullException("dataReader");

            _dataReader = dataReader;
            _nonSerializedFields = nonSerializedFields;
            _fields = GetFields(_dataReader);
        }

        public IDataReader DataReader
        {
            get { return _dataReader; }
        }

        public string[] NonSerializedFields
        {
            get { return _nonSerializedFields; }
            set { _nonSerializedFields = value; }
        }

        private ICollection<string> GetFields(IDataReader dataReader)
        {
            ICollection<string> list = new List<string>();
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                list.Add(dataReader.GetName(i));
            }
            return list;
        }

        #region IGISAttributes Members

        public object GetValue(string attribute)
        {
            return _dataReader[attribute];
        }

        public void SetValue(string attribute, object value)
        {
            throw new InvalidOperationException( "This data source does not support setting property values." );
        }

        public IEnumerable<string> GetKeys()
        {
            return _fields.ToArray<string>();
        }

        #endregion

        #region IGISXMLSerializable Members

        public void ToXML(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGISJSONSerializable Members

        public void ToJSON(Jayrock.Json.JsonTextWriter jwriter)
        {
            GeoJSONWriter.Write(this, jwriter, _nonSerializedFields);
        }

        #endregion
    }
}
