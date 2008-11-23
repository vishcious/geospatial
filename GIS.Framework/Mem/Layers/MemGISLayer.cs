using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GIS.Framework.Mem.Features;
using Jayrock.Json;

namespace GIS.Framework.Mem.Layers
{
    public abstract class MemGISLayer: IGISLayer, IGISEditableLayer, IGISXMLReadable, IGISJSONReadable
    {
        List<MemFeature> _source = new List<MemFeature>();
        List<MemFeature> _features;
        string _layerName;
        string _keyFieldName;
        int _index = -1;
        MemFeature _current;
        IEnumerable<KeyValuePair<string, object>> _query = null;

        public MemGISLayer(string layerName, string keyFieldName)
        {
            _layerName = layerName;
            _keyFieldName = keyFieldName;
        }

        public MemGISLayer(IEnumerable<MemFeature> features, string layerName, string keyFieldName)
        {
            _source = new List<MemFeature>(features);
            _layerName = layerName;
            _keyFieldName = keyFieldName;
        }
        public abstract MemFeature CreateFeature();

        #region IGISLayer Members

        public string LayerName
        {
            get { return _layerName; }
        }

        public string KeyFieldName
        {
            get { return _keyFieldName; }
        }

        public IGISFeature Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return _current;
            }
        }

        public bool MoveNext()
        {
            if (_features == null || ++_index >= _features.Count)
            {
                return false;
            }
            else
            {
                _current = _features[_index];
                return true;
            }
        }

        /// <summary>
        /// Searches the layer using specified query.
        /// The query is represented in a list of key-value pairs where the KEY represents the field name to match
        /// and the VALUE represents the value to the matched in the corresponding field.
        /// The results collection is formed by features that match all specified conditions.
        /// and initializes the layer with the search results collection.
        /// </summary>
        /// <param name="query">The query.</param>
        public void Search(object query)
        {
            if (query == null)
            {
                _query = null;
                _features = _source;
                return;
            }

            if (!(query is IDictionary<string, object>))
                throw new ArgumentException("The query should be of type[IDictionary<string, object>].", "query");

            Search(query as IDictionary<string, object>);
        }

        #endregion

        /// <summary>
        /// Searches the layer using specified query.
        /// The query is represented in a list of key-value pairs where the KEY represents the field name to match
        /// and the VALUE represents the value to the matched in the corresponding field.
        /// and initializes the layer with the search results collection.
        /// </summary>
        /// <param name="conditions">The conditions.</param>
        public void Search(IEnumerable<KeyValuePair<string, object>> conditions)
        {
            if (conditions == null)
            {
                _query = null;
                _features = _source;
                return;
            }

            _query = conditions;

            MemGISFeatureFinder finder = new MemGISFeatureFinder(conditions);
            _features = _source.FindAll(finder.IsMatch);
            _index = -1;
        }

        #region IGISXMLSerializable Members

        public void ToXML(System.Xml.XmlWriter writer)
        {
            GeoXMLWriter.Write(this, writer);
        }

        public void FromXML(System.Xml.XmlReader reader)
        {
            _source = new List<MemFeature>();

            reader.MoveToContent();
            if (reader.IsStartElement("Layer"))
            {
                reader.ReadStartElement("Layer");
                _layerName = reader.GetAttribute("Name");
                _keyFieldName = reader.GetAttribute("KeyField");

                while (reader.IsStartElement("Feature"))
                {
                    MemFeature feature = CreateFeature();
                    feature.FromXML(reader);
                    Add(feature);
                }

                reader.ReadEndElement();
            }
        }

        #endregion

        #region IGISJSONSerializable Members

        public void ToJSON(Jayrock.Json.JsonTextWriter jwriter)
        {
            GeoJSONWriter.Write(this, jwriter);
        }

        public void FromJSON(Jayrock.Json.JsonTextReader jreader)
        {
            if (jreader == null)
                throw new ArgumentNullException("jreader", "A valid JSON reader object is required.");

            if (jreader.MoveToContent() && jreader.TokenClass == JsonTokenClass.Object)
            {
                jreader.ReadToken(JsonTokenClass.Object);

                    //Read the 'FeatureCollection' as the type
                    jreader.ReadMember(); //reads 'type'
                    jreader.ReadString(); //reads 'FeatureCollection'

                    //Read the features
                    jreader.ReadMember();
                    jreader.ReadToken(JsonTokenClass.Array);
                    while (jreader.TokenClass == JsonTokenClass.Object)
                    {
                        MemFeature feature = CreateFeature();
                        GeoJSONReader.ReadGISFeature(feature, jreader);
                        Add(feature);
                    }
                    jreader.ReadToken(JsonTokenClass.EndArray);

                    //Read the layer name
                    jreader.ReadMember();
                    _layerName = jreader.ReadString();

                    //Read the key field
                    jreader.ReadMember();
                    _keyFieldName = jreader.ReadString();

                jreader.ReadToken(JsonTokenClass.EndObject);
            }
        }

        #endregion

        #region IGISEditableLayer Members

        public void Add(IGISFeature feature)
        {
            if (feature == null)
                throw new ArgumentNullException("feature", "A valid feature is required to be added.");

            if (_features == null)
                throw new NullReferenceException("Cannot add feature to the InMemoryLayer. The internal list reference is NULL.");

            if(FindByKeyValue(_features, feature.Attributes.GetValue(KeyFieldName)) != null)
                throw new ArgumentException("Cannot add feature. A feature with the same key already exists.");

            MemFeature newFeature = CreateFeature();
            _features.Add(newFeature);

            newFeature.Shape = feature.Shape;

            foreach (string key in feature.Attributes.GetKeys())
            {
                newFeature.Attributes.SetValue(key, feature.Attributes.GetValue(key));
            }
        }

        public void Update(IGISFeature feature)
        {
            if (feature == null)
                throw new ArgumentNullException("feature", "A valid feature is required to be added.");

            if (_features == null)
                throw new NullReferenceException("Cannot update feature to the InMemoryLayer. The internal list reference is NULL.");

            if (FindByKeyValue(_features, feature.Attributes.GetValue(KeyFieldName)) != null)
                throw new ArgumentException("Cannot update feature. A feature with the same key already exists.");

            MemFeature updateFeature = FindByKeyValue(_features, feature.Attributes.GetValue(KeyFieldName));
            if (updateFeature == null)
                throw new ArgumentException("Could not update the InMemoryLayer. The feature was not found in the internal list.");

            updateFeature.Shape = feature.Shape;

            foreach (string key in feature.Attributes.GetKeys())
            {
                updateFeature.Attributes.SetValue(key, feature.Attributes.GetValue(key));
            }
        }

        public void Delete(IGISFeature feature)
        {
            if (feature == null)
                throw new ArgumentNullException("feature", "A valid feature is required to be added.");

            if (_features == null)
                throw new NullReferenceException("Cannot delete feature from the InMemoryLayer. The internal list reference is NULL.");

            MemFeature mc = FindByKeyValue(_features, feature.Attributes.GetValue(KeyFieldName));

            if (mc != null)
            {
                _features.Remove(mc);
            }
        }

        #endregion

        private MemFeature FindByKeyValue(List<MemFeature> features, object id)
        {
            //Dictionary<string, object> conditions = new Dictionary<string, object>();
            //conditions.Add(KeyFieldName, id);
            //MemGISFeatureFinder finder = new MemGISFeatureFinder(conditions);
            //return features.Find(finder.IsMatch);
            foreach (MemFeature item in features)
            {
                if (id == item.Attributes.GetValue(KeyFieldName))
                    return item;
            }
            return null;
        }

        public IEnumerable<KeyValuePair<string, object>> GetIDQuery(object value)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add(KeyFieldName, value);
            return conditions;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _features = null;
            _source = null;
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
