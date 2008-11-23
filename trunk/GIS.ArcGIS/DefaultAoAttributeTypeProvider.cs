using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESRI.ArcGIS.Geodatabase;

namespace GIS.ArcGIS
{
    public class DefaultAoAttributeTypeProvider : IAoAttributeTypeProvider
    {
        #region IAttributeTypeProvider Members

        public System.Data.DataColumn GetAttributeType(IField2 field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeBlob)
                return new System.Data.DataColumn(field.Name, typeof(byte[]));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDate)
                return new System.Data.DataColumn(field.Name, typeof(DateTime));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
                return new System.Data.DataColumn(field.Name, typeof(double));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGlobalID)
                return new System.Data.DataColumn(field.Name, typeof(Guid));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGUID)
                return new System.Data.DataColumn(field.Name, typeof(Guid));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger)
                return new System.Data.DataColumn(field.Name, typeof(Int32));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID)
                return new System.Data.DataColumn(field.Name, typeof(Int32));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeRaster)
                return new System.Data.DataColumn(field.Name, typeof(System.Drawing.Image));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSingle)
                return new System.Data.DataColumn(field.Name, typeof(Single));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger)
                return new System.Data.DataColumn(field.Name, typeof(Single));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString)
                return new System.Data.DataColumn(field.Name, typeof(string));
            else if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeXML)
                return new System.Data.DataColumn(field.Name, typeof(byte[]));

            return null;
        }

        public IField2 GetAttributeType(DataColumn geometryColumn)
        {
            if (geometryColumn == null)
                throw new ArgumentNullException("geometryColumn");

            IField2 field = null;

            if (geometryColumn.DataType == typeof(byte[]))
            {
                field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = geometryColumn.ColumnName;
                fieldEdit.AliasName_2 = geometryColumn.ColumnName;
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeBlob;
            }
            else if (geometryColumn.DataType == typeof(DateTime))
            {
                field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = geometryColumn.ColumnName;
                fieldEdit.AliasName_2 = geometryColumn.ColumnName;
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
            }
            else if (geometryColumn.DataType == typeof(double))
            {
                field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = geometryColumn.ColumnName;
                fieldEdit.AliasName_2 = geometryColumn.ColumnName;
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
            }
            else if (geometryColumn.DataType == typeof(Guid) && geometryColumn.Unique)
            {
                field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = geometryColumn.ColumnName;
                fieldEdit.AliasName_2 = geometryColumn.ColumnName;
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeGlobalID;
            }
            else if (geometryColumn.DataType == typeof(Guid) && !geometryColumn.Unique)
            {
                field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = geometryColumn.ColumnName;
                fieldEdit.AliasName_2 = geometryColumn.ColumnName;
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeGUID;
            }
            else if (geometryColumn.DataType == typeof(Int32) && geometryColumn.Unique)//OID
            {
                field = new FieldClass();
                IFieldEdit fieldEditOID = (IFieldEdit)field;
                fieldEditOID.Name_2 = geometryColumn.ColumnName;
                fieldEditOID.AliasName_2 = geometryColumn.ColumnName;
                fieldEditOID.Type_2 = esriFieldType.esriFieldTypeOID;
            }
            else if (geometryColumn.DataType == typeof(Int32) && !geometryColumn.Unique)
            {
                field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = geometryColumn.ColumnName;
                fieldEdit.AliasName_2 = geometryColumn.ColumnName;
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
            }
            else if (geometryColumn.DataType == typeof(Single))
            {
                field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = geometryColumn.ColumnName;
                fieldEdit.AliasName_2 = geometryColumn.ColumnName;
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeSingle;
            }
            else if (geometryColumn.DataType == typeof(string))
            {
                field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = geometryColumn.ColumnName;
                fieldEdit.AliasName_2 = geometryColumn.ColumnName;
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit.Length_2 = geometryColumn.MaxLength;
            }
            else if( geometryColumn.DataType == typeof( System.Drawing.Image ) )
            {
                field = new FieldClass();
                IFieldEdit fieldEdit = ( IFieldEdit ) field;
                fieldEdit.Name_2 = geometryColumn.ColumnName;
                fieldEdit.AliasName_2 = geometryColumn.ColumnName;
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeRaster;
            }

            return field;
        }

        #endregion
    }
}
