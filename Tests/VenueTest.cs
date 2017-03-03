using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BandTracker
{
    public class VenueTest : IDisposable
    {
        public VenueTest()
        {
          DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
      }

      [Fact]
      public void Test_VenuesEmptyAtFirst()
      {
          //Arrange, Act
          int result = Venue.GetAll().Count;

          //Assert
          Assert.Equal(0, result);
      }

      [Fact]
      public void Test_Equal_ReturnsTrueForSameName()
      {
          //Arrange, Act
          Venue firstVenue = new Venue("Gator Lounge");
          Venue secondVenue = new Venue("Gator Lounge");

          //Assert
          Assert.Equal(firstVenue, secondVenue);
      }

      [Fact]
      public void Test_Save_SavesVenueToDatabase()
      {
          //Arrange
          Venue testVenue = new Venue("Gator Lounge");
          testVenue.Save();

          //Act
          List<Venue> result = Venue.GetAll();
          List<Venue> testList = new List<Venue>{testVenue};

          //Assert
          Assert.Equal(testList, result);
      }

      [Fact]
      public void Test_Save_AssignsIdToVenueObject()
      {
          //Arrange
          Venue testVenue = new Venue("Gator Lounge");
          testVenue.Save();

          //Act
          Venue savedVenue = Venue.GetAll()[0];

          int result = savedVenue.GetId();
          int testId = testVenue.GetId();

          //Assert
          Assert.Equal(testId, result);
      }

      public void Dispose()
      {
          Venue.DeleteAll();
          Band.DeleteAll();
      }

    }
}
