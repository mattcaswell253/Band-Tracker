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

      [Fact]
      public void Test_Find_FindsVenueInDatabase()
      {
          //Arrange
          Venue testVenue = new Venue("Asian");
          testVenue.Save();

          //Act
          Venue foundVenue = Venue.Find(testVenue.GetId());

          //Assert
          Assert.Equal(testVenue, foundVenue);
      }


      [Fact]
      public void Test_GetBands_RetrievesAllBandsInVenue()
      {
          //Arrange
          Venue testVenue = new Venue("Gator Lounge");
          testVenue.Save();
          Band firstBand = new Band("Nirvana");
          firstBand.Save();
          Band secondBand = new Band("Cake");
          secondBand.Save();

          //Act
          testVenue.AddBand(firstBand);
          testVenue.AddBand(secondBand);
          List<Band> testBandList = new List<Band> {firstBand, secondBand};
          List<Band> resultBandList = testVenue.GetBands();

          //Assert
          Assert.Equal(testBandList, resultBandList);
      }


      [Fact]
      public void Test_Delete_DeletesVenueAssociationsFromDatabase()
      {
          //Arrange
          Band testBand = new Band("Nirvana");
          testBand.Save();

          string testName = "Gator Lounge";
          Venue testVenue = new Venue(testName);
          testVenue.Save();

          //Act
          testVenue.AddBand(testBand);
          testVenue.Delete();

          List<Venue> resultBandVenues = testBand.GetVenues();
          List<Venue> testBandVenues = new List<Venue>{};

          //Assert
          Assert.Equal(testBandVenues, resultBandVenues);
      }

      public void Dispose()
      {
          Venue.DeleteAll();
          Band.DeleteAll();
      }

    }
}
