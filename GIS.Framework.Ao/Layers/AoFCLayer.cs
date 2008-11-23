using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using GIS.ArcGIS;

namespace GIS.Framework.Ao.Layers
{
    public abstract class AoFCLayer : IGISLayer, ISupportsGISFields, ISupportsSRID
    {
        IFeatureClass _featureClass;
        IFeatureCursor _featureCursor;
        IFeature _feature;
        string _layerName;
        string _keyFieldName;
        IQueryFilter _query;
        AoGISFields _gisFields;

        public AoFCLayer(IFeatureClass featureClass, string layerName, string keyFieldName)
        {
            if (featureClass == null)
                throw new ArgumentNullException("featureClass");
            if (string.IsNullOrEmpty(layerName))
                throw new ArgumentNullException("layerName");
            if (string.IsNullOrEmpty(keyFieldName))
                throw new ArgumentNullException("keyFieldName");

            _gisFields = new AoGISFields(featureClass.Fields);

            _featureClass = featureClass;
            _layerName = layerName;
            _keyFieldName = keyFieldName;
        }

        protected IFeatureClass GetFeatureClass()
        {
            return _featureClass;
        }

        protected IFeatureCursor GetFeatureCursor()
        {
            return _featureCursor;
        }

        #region IGISLayer Members

        public  string LayerName
        {
            get { return _layerName; }
        }

        public string KeyFieldName
        {
            get { return _keyFieldName; }
        }

        public abstract IGISFeature CreateFeature(IFeature feature);

        public IGISFeature Current
        {
            get
            {
                if (_feature != null)
                    return CreateFeature(_feature);

                return null;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                if (_feature != null)
                    return CreateFeature(_feature);

                return null;
            }
        }

        public bool MoveNext()
        {
            if (_featureCursor == null)
                return false;
            else
            {
                _feature = _featureCursor.NextFeature();
                if (_feature != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Searches the FeatureClass using the specified queryfilter.
        /// </summary>
        /// <param name="queryfilter">The query should be of type [IQueryfilter].</param>
        public void Search(object queryFilter)
        {
            if (queryFilter != null)
            {
                if (!(queryFilter is IQueryFilter))
                    throw new ArgumentException("The query should be of type [IQueryFilter].", "queryfilter");
            }

            _query = queryFilter as IQueryFilter;

            _featureCursor = _featureClass.Search(_query, true);
            _gisFields = new AoGISFields(_featureCursor.Fields);
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
            _featureCursor = null;
            _featureClass = null;
        }

        #endregion

        #region IEnumerator Members


        public void Reset()
        {
            Search(_query);
        }

        #endregion

        public IFields Fields
        {
            get
            {
                if (_featureClass == null)
                    return null;
                else
                    return _featureClass.Fields;
            }
        }


        #region ISupportsGISFields Members

        public IGISFields GetGISFields()
        {
            return _gisFields;
        }

        #endregion

        #region ISupportsSRID Members

        public int? Srid
        {
            get
            {
                IGeoDataset geoDataset = _featureClass as IGeoDataset;
                if( geoDataset != null )
                {
                    if( geoDataset.SpatialReference != null )
                        return geoDataset.SpatialReference.FactoryCode;
                }
                return null;
            }
            set
            {
                throw new InvalidOperationException( "Does not support setting the spatial reference." );
            }
        }

        #endregion
    }
}
