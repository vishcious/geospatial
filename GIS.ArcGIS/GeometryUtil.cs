using System;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GIS.ArcGIS
{
    /// <summary>
    /// Static helper class for ArcGIS geometry operations.
    /// </summary>
    public class GeometryUtil
    {

        /// <summary>
        /// Compares the spatial references.
        /// </summary>
        /// <param name="sourceSpatialReference">The source spatial reference.</param>
        /// <param name="targetSpatialReference">The target spatial reference.</param>
        /// <returns><b>True</b> if spatial references are the same (coordinate system and XY precision); otherwise <b>False</b>.</returns>
        public static bool CompareSpatialReferences(ISpatialReference sourceSpatialReference, ISpatialReference targetSpatialReference)
        {

            // Clone the spatialreferences so as not to modify it
            IClone sourceClone;
            sourceClone = (IClone)sourceSpatialReference;

            IClone targetClone;
            targetClone = (IClone)targetSpatialReference;

            //
            // Compare the coordinate system component of the 
            // spatial reference
            //
            bool areEqual;
            areEqual = sourceClone.IsEqual(targetClone);

            // If the comparison failed, return false and exit
            if (!areEqual) return false;

            //
            // Also, compare the XY precision to ensure the 
            // spatial references are equal
            //
            // QI to get the ISpatialReference2 interface
            ISpatialReference2GEN sourceSpatialReference2;
            //Supports Non-OLE Automation-Compliant Types
            sourceSpatialReference2 = (ISpatialReference2GEN)sourceSpatialReference;

            // Do the xy precision check
            bool xyPrecisionIsEqual;
            xyPrecisionIsEqual = sourceSpatialReference2.IsXYPrecisionEqual(targetSpatialReference);

            // If the comparison failed, return false and exit
            if (!xyPrecisionIsEqual)
            {
                return false;
            }

            // OK, they must be Equal
            return true;

        }

        public static bool IsNonLinearGeometry(ISegmentCollection segColl)
        {
            IEnumSegment enumSeg = segColl.EnumSegments;
            enumSeg.Reset();
            ISegment seg;
            int outPartIdx = 0;
            int segIdx = 0;
            enumSeg.Next(out seg, ref outPartIdx, ref segIdx);

            ESRI.ArcGIS.Geometry.esriGeometryType geometryType;
            while (seg != null)
            {
                geometryType = seg.GeometryType;
                if (geometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryBezier3Curve ||
                    geometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryCircularArc ||
                    geometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryEllipticArc)
                {
                    return true;
                }
                enumSeg.Next(out seg, ref outPartIdx, ref segIdx);
            }
            return false;


            //IEnumSegment enumSeg = segColl.EnumSegments;
            //enumSeg.Reset();
            //ISegment seg = enumSeg.Next();

            //do while (seg != null)
            //{
            //    ESRI.ArcGIS.Geometry.esriGeometryType geometryType = seg.GeometryType;
            //    if (geometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryBezier3Curve ||
            //        geometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryCircularArc ||
            //        geometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryEllipticArc)
            //    {
            //        return true;
            //    }
            //seg = enumSeg.Next();
            //}
            //return false;
        }

		/// <summary>
		/// Simplifies the geometry.
		/// </summary>
		/// <param name="inputGeometry">The input geometry.</param>
		/// <returns></returns>
		public static IGeometry SimplifyGeometry(IGeometry inputGeometry)
		{
			// Validate the arguments
			if (inputGeometry == null)
				throw new ArgumentNullException("inputGeometry", "Failed to simplify geometry. Input geometry has not been set.");

			ITopologicalOperator2 topologicalOperator = inputGeometry as ITopologicalOperator2;
			if (topologicalOperator == null)
				throw new ArgumentException("Failed to simplify geometry. Input geometry must be a polyline, polygon, multipoint or multipatch.");

			// Simplify the geometry
			topologicalOperator.IsKnownSimple_2 = false;
			topologicalOperator.Simplify();

			// Return simple geometry
			return inputGeometry;

		}

		/// <summary>
		/// Buffers the input geometry.
		/// </summary>
		/// <param name="inputGeometry">The input geometry.</param>
		/// <param name="bufferDistance">The buffer distance.</param>
		public static void BufferGeometry(ref IGeometry inputGeometry, double bufferDistance)
		{
			if (inputGeometry == null) return;
			if (inputGeometry.IsEmpty) return;

			IGeometry bufferGeometry = null;

            //// DEBUG - Check area before
            //if (inputGeometry is IPolygon)
            //{
            //    IArea areaBefore = inputGeometry as IArea;
            //    System.Diagnostics.Debug.WriteLine(areaBefore.Area);
            //}
			
			// Get the ITopologicalOperator
			ITopologicalOperator topoOp = (ITopologicalOperator)inputGeometry;

			// Perform buffer
			bufferGeometry = topoOp.Buffer(bufferDistance);

            //// DEBUG - Check area after
            //if (bufferGeometry is IPolygon)
            //{
            //    IArea areaAfter = bufferGeometry as IArea;
            //    System.Diagnostics.Debug.WriteLine("After Buffer:  " + areaAfter.Area);
            //}

			inputGeometry = bufferGeometry;
		}

        /// <summary>
        /// Gets the projected spatial reference.
        /// </summary>
        /// <param name="projection">The spatial reference projection type; i.e. factory code. Use an element <b>value</b> from the <see cref="ESRI.ArcGIS.Geometry.esriSRProjCSType"/>, 
        /// <see cref="ESRI.ArcGIS.Geometry.esriSRProjCS2Type"/>, <see cref="ESRI.ArcGIS.Geometry.esriSRProjCS3Type"/>, or 
        /// <see cref="ESRI.ArcGIS.Geometry.esriSRProjCS4Type"/> enumerations as the projectedCoordinateSystemType to create a particular 
        /// predefined projected coordinate system.
        /// <para>This is the <b>value</b> for a given projected coordinate system constant.</para>
        /// </param>
        /// <returns></returns>
        public static ISpatialReference GetProjectedSpatialReference(int projectedCoordinateSystemType)
        {
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            IProjectedCoordinateSystem projectedCoordinateSystem = spatialReferenceFactory.CreateProjectedCoordinateSystem(projectedCoordinateSystemType);
            return projectedCoordinateSystem;
        }

        /// <summary>
        /// Gets the geographic spatial reference.
        /// </summary>
        /// <param name="projection">The spatial reference geographic coordinate system type; i.e. factory code. Use an element <b>value</b> from the 
        /// <see cref="ESRI.ArcGIS.Geometry.esriSRGeoCSType "/>, <see cref="ESRI.ArcGIS.Geometry.esriSRGeoCS2Type "/>, 
        /// or <see cref="ESRI.ArcGIS.Geometry.esriSRGeoCS3Type "/> enumerations as the geographicCoordinateSystemType to create a particular 
        /// predefined geographic coordinate system.
        /// <para>This is the <b>value</b> for a given geographic coordinate system constant.</para>
        /// </param>
        /// <returns></returns>
        public static ISpatialReference GetGeographicSpatialReference(int geographicCoordinateSystemType)
        {
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            IGeographicCoordinateSystem geographicCoordinateSystem = spatialReferenceFactory.CreateGeographicCoordinateSystem(geographicCoordinateSystemType);
            return geographicCoordinateSystem;
        }
	}
}
