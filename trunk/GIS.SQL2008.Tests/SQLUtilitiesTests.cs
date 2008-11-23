using GIS.Framework.Mem.Features;
using System;
using MbUnit.Framework;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using GIS.SQL2008;
using GIS.Framework;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace GIS.SQL2008.Tests
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="SQLUtilities"/> class.
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(SQLUtilities))]
    public class SQLUtilitiesTest
    {
        #region Fields, SetUp and TearDown
        
        /// <summary>
        /// Sets up the fixture
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            
        }

        /// <summary>
        /// Cleans up the fixture
        /// </summary>
        [TearDown]
        public void TearDown()
        {

        }

        #endregion
    
        #region Test cases

        [Test]
        public void SQLforGeographyTest()
        {
            IGeometry geometry = new Polygon( null, null, new GeometryFactory() );
            ICoordinate[] coords = new List<Coordinate>() { new Coordinate( 90, -90 ), new Coordinate( -90, 90 ), new Coordinate( 180, -90 ), new Coordinate( 90, -90 ) }.ToArray();
            geometry = new Polygon( new LinearRing(coords) );
            
            string result = SQLUtilities.SQLforGeography( geometry, null );
            Assert.IsNotNull( result, "Error generating SQL for geography." );

            result = SQLUtilities.SQLforGeography( geometry, 4326 );
            Assert.IsNotNull( result, "Error generating SQL for geography." );
        }

        [Test]
        public void SQLforGeometryTest()
        {
            IGeometry geometry = new Polygon( null, null, new GeometryFactory() );
            ICoordinate[] coords = new List<Coordinate>() { new Coordinate( 90, -90 ), new Coordinate( -90, 90 ), new Coordinate( 180, -90 ), new Coordinate( 90, -90 ) }.ToArray();
            geometry = new Polygon( new LinearRing( coords ) );

            string result = SQLUtilities.SQLforGeometry( geometry, null );
            Assert.IsNotNull( result, "Error generating SQL for geometry." );

            result = SQLUtilities.SQLforGeography( geometry, 4326 );
            Assert.IsNotNull( result, "Error generating SQL for geometry." );
        }

        [Test]        
        public void CreateGeographyInsertCommandTest()
        {
            DataColumn dc1 = new DataColumn( "ID", typeof( int ) );
            DataColumn dc2 = new DataColumn( "GEOG", typeof( SqlGeography ) );
            DataColumn dc3 = new DataColumn( "Name", typeof( string ) );

            List<DataColumn> list = new List<DataColumn>() { dc1, dc2, dc3 };

            ICoordinate[] coords = new List<Coordinate>() { new Coordinate( 90, -90 ), new Coordinate( -90, 90 ), new Coordinate( 180, -90 ), new Coordinate( 90, -90 ) }.ToArray();

            MemPolygonFeature feature = new MemPolygonFeature( new LinearRing( coords ), null );
            feature.Attributes.SetValue( "ID", 123 );
            feature.Attributes.SetValue( "Name", "Vish" );

            SqlCommand sqlCommand = SQLUtilities.CreateGeographyInsertCommand( "GeographyTable", "SHAPE", list.GetEnumerator(), feature );
            Assert.IsNotNull( sqlCommand, "Failed to generate INSERT geography command SQL." );
            Assert.AreEqual( sqlCommand.Parameters.Count, 4, "INSERT SQL parameter count does not match" );

            dc1.AutoIncrement = true;

            sqlCommand = SQLUtilities.CreateGeographyInsertCommand( "GeographyTable", "SHAPE", list.GetEnumerator(), feature );
            Assert.IsNotNull( sqlCommand, "Failed to generate INSERT geography command SQL." );
            Assert.AreEqual( sqlCommand.Parameters.Count, 3, "INSERT SQL with auto increment column parameter count does not match" );
        }

        [Test]
        public void CreateGeometryInsertCommandTest()
        {
            DataColumn dc1 = new DataColumn( "ID", typeof( int ) );
            DataColumn dc2 = new DataColumn( "GEOG", typeof( SqlGeometry ) );
            DataColumn dc3 = new DataColumn( "Name", typeof( string ) );

            List<DataColumn> list = new List<DataColumn>() { dc1, dc2, dc3 };

            ICoordinate[] coords = new List<Coordinate>() { new Coordinate( 90, -90 ), new Coordinate( -90, 90 ), new Coordinate( 180, -90 ), new Coordinate( 90, -90 ) }.ToArray();

            MemPolygonFeature feature = new MemPolygonFeature( new LinearRing( coords ), null );
            feature.Attributes.SetValue( "ID", 123 );
            feature.Attributes.SetValue( "Name", "Vish" );

            SqlCommand sqlCommand = SQLUtilities.CreateGeometryInsertCommand( "GeometryTable", "SHAPE", list.GetEnumerator(), feature );
            Assert.IsNotNull( sqlCommand, "Failed to generate INSERT geography command SQL." );
            Assert.AreEqual( sqlCommand.Parameters.Count, 4, "INSERT SQL parameter count does not match" );

            dc1.AutoIncrement = true;

            sqlCommand = SQLUtilities.CreateGeometryInsertCommand( "GeometryTable", "SHAPE", list.GetEnumerator(), feature );
            Assert.IsNotNull( sqlCommand, "Failed to generate INSERT geography command SQL." );
            Assert.AreEqual( sqlCommand.Parameters.Count, 3, "INSERT SQL with auto increment column parameter count does not match" );
        }

        #endregion    
    }
}
	