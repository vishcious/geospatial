using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Data;
using ESRI.ArcGIS.Geometry;
using GIS.ArcGIS;
using GIS.Framework;
using System.Collections;

namespace GIS.ArcGIS
{
    public delegate string GetGeometryType(esriGeometryType geometryType);

    public class AoGISFields: IGISFields, IDisposable
    {
        string _geometryType = null;
        IList<DataColumn> _attributeFields = new List<DataColumn>();
        IAoGeometryTypeProvider _geometryTypeProvider = new DefaultAoGeometryTypeProvider();
        IAoAttributeTypeProvider _attributeTypeProvider = new DefaultAoAttributeTypeProvider();

        public AoGISFields(IFields fields)
        {
            if (fields == null)
                throw new ArgumentNullException("fields");

            int length = fields.FieldCount;
            for (int i = 0; i < length; i++)
            {
                IField field = fields.get_Field(i);

                if (field.Type == esriFieldType.esriFieldTypeGeometry)
                {
                    _geometryType = GeometryTypeProvider.GetGeometryType(field.GeometryDef.GeometryType);
                }
                else
                {
                    DataColumn col = AttributeTypeProvider.GetAttributeType(field as IField2);
                    if (col != null)
                        _attributeFields.Add(col);
                }
            }

            if (_geometryType == null)
            {
                _attributeFields = null;
                throw new ArgumentException("The geometry field is missing or the geometry type is not supported.", "fields");
            }
        }

        public IAoGeometryTypeProvider GeometryTypeProvider
        {
            get
            {
                return _geometryTypeProvider;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _geometryTypeProvider = value;
            }
        }

        public IAoAttributeTypeProvider AttributeTypeProvider
        {
            get
            {
                return _attributeTypeProvider;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _attributeTypeProvider = value;
            }
        }

        public int AttributeFieldCount
        {
            get
            {
                return _attributeFields.Count;
            }
        }

        public IField2 GetGeometryField(string geometryType, string geometryFieldName)
        {
            esriGeometryType type = _geometryTypeProvider.GetGeometryType(geometryType);
            // Create the geometry field
            IGeometryDef geometryDef = new GeometryDefClass();
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;

            // Assign Geometry Definition
            geometryDefEdit.GeometryType_2 = type;
            geometryDefEdit.GridCount_2 = 1;
            geometryDefEdit.set_GridSize(0, 0.5);
            geometryDefEdit.AvgNumPoints_2 = 2;
            geometryDefEdit.HasM_2 = false;
            geometryDefEdit.HasZ_2 = false;

            // Create Geometry Field
            IField fieldShape = new FieldClass();
            IFieldEdit fieldEditShape = (IFieldEdit)fieldShape;
            fieldEditShape.Name_2 = geometryFieldName;
            fieldEditShape.AliasName_2 = geometryFieldName;
            fieldEditShape.Type_2 = esriFieldType.esriFieldTypeGeometry;
            fieldEditShape.GeometryDef_2 = geometryDef;

            return null;
        }

        #region IGISFields Members

        public string GeometryType
        {
            get { return _geometryType; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _attributeFields = null;
            _geometryTypeProvider = null;
            _attributeTypeProvider = null;
        }

        #endregion

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
