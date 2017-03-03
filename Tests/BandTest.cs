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




        public void Dispose()
      {

          Band.DeleteAll();
      }


    }
}
