using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BandTracker
{
    public class BandTrackerTest : IDisposable
    {
        public BandTrackerTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_DatabaseEmptyAtFirst()
        {
          //Arrange, Act
          int result = Band.GetAll().Count;

          //Assert
          Assert.Equal(0, result);
        }

        [Fact]
      public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
      {
          //Arrange, Act
          Band firstBand = new Band("Nirvana");
          Band secondBand = new Band("Nirvana");


          //Assert
          Assert.Equal(firstBand, secondBand);
      }

      [Fact]
      public void Test_Save_SavesToDatabase()
      {
          //Arrange
          Band testBand = new Band("Nirvana");

          //Act
          testBand.Save();
          List<Band> result = Band.GetAll();
          List<Band> testList = new List<Band>{testBand};

          //Assert
          Assert.Equal(testList, result);
        }


        [Fact]
        public void Test_Save_AssignsIdToObject()
        {
            //Arrange
            Band testBand = new Band("Nirvana");

            //Act
            testBand.Save();
            Band savedBand = Band.GetAll()[0];


            int result = savedBand.GetId();
            int testId = testBand.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_Find_FindBandInDatabase()
        {
            //Arrange
            Band testBand = new Band("Nirvana");
            testBand.Save();

            //Act
            Band foundBand = Band.Find(testBand.GetId());

            //Assert
            Assert.Equal(testBand, foundBand);
        }


        [Fact]
        public void Test_AddVenue_AddsVenueToBand()
        {
          //Arrange
          Band testBand = new Band("Nirvana");
          testBand.Save();

          Venue testVenue = new Venue("White River Ampitheater");
          testVenue.Save();

          //Act
          testBand.AddVenue(testVenue);

          List<Venue> result = testBand.GetVenues();
          List<Venue> testList = new List<Venue>{testVenue};

          //Assert
          Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_GetVenue_ReturnsAllBandVenues()
        {
          //Arrange
          Band testBand = new Band("Nirvana");
          testBand.Save();

          Venue testVenue1 = new Venue("ShowBox SODO");
          testVenue1.Save();

          Venue testVenue2 = new Venue("Gator Lounge");
          testVenue2.Save();

          //Act
          testBand.AddVenue(testVenue1);
          List<Venue> result = testBand.GetVenues();
          List<Venue> testList = new List<Venue> {testVenue1};

          //Assert
          Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_Delete_DeletesBandFromVenue()
        {
          //Arrange
          Venue testVenue = new Venue("White River Ampitheater");
          testVenue.Save();

          Band testBand = new Band("Nirvana");
          testBand.Save();

          //Act
          testBand.AddVenue(testVenue);
          testBand.Delete();

          List<Band> resultVenueBands = testVenue.GetBands();
          List<Band> testVenueBands = new List<Band> {};

          //Assert
          Assert.Equal(testVenueBands, resultVenueBands);
        }

        public void Dispose()
      {
          Venue.DeleteAll();
          Band.DeleteAll();
      }


    }
}
