using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace BandTracker
{
  public class Band
  {
    private int _id;
    private string _name;

    public Band(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        bool idEquality = this.GetId() == newBand.GetId();
        bool nameEquality = this.GetName() == newBand.GetName();
        return (idEquality && nameEquality);
      }
    }

    public int GetId()
  {
    return _id;
  }
  public string GetName()
  {
    return _name;
  }

    public static List<Band> GetAll()
    {
      List<Band> allBands = new List<Band>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int BandId = rdr.GetInt32(0);
        string BandName = rdr.GetString(1);
        Band newBand = new Band(BandName, BandId);
        allBands.Add(newBand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allBands;
    }

    public void Save()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@BandName);", conn);

    cmd.Parameters.Add(new SqlParameter("@BandName", this.GetName()));

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Band Find(int id)
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @BandId;", conn);

    cmd.Parameters.Add(new SqlParameter("@BandId", id.ToString()));

    SqlDataReader rdr = cmd.ExecuteReader();

    int foundBandId = 0;
    string foundBandName = null;

    while(rdr.Read())
    {
      foundBandId = rdr.GetInt32(0);
      foundBandName = rdr.GetString(1);
    }
    Band foundBand = new Band(foundBandName, foundBandId);

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return foundBand;
  }
//
//   public List<Venue> GetVenues()
// {
//   SqlConnection conn = DB.Connection();
//   conn.Open();
//
//   SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands JOIN bands_venues ON (bands.id = venues_bands.band_id) JOIN categories ON (venues_bands.venue_id = venues.id) WHERE bands.id = @BandId;", conn);
//
//   cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId().ToString()));
//
//   SqlDataReader rdr = cmd.ExecuteReader();
//
//   List<Venue> venues = new List<Venue>{};
//
//   while(rdr.Read())
//   {
//     int venueId = rdr.GetInt32(0);
//     string venueName = rdr.GetString(1);
//     Venue newVenue = new Venue(venueName, venueId);
//     venues.Add(newVenue);
//   }
//
//   if (rdr != null)
//   {
//     rdr.Close();
//   }
//   if (conn != null)
//   {
//     conn.Close();
//   }
//   return venues;
// }

public void Delete()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id = @BandId; DELETE FROM categories_bands WHERE band_id = @BandId;", conn);

    cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId()));

    cmd.ExecuteNonQuery();

    if (conn != null)
    {
      conn.Close();
    }
  }

  public static void DeleteAll()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();
    SqlCommand cmd = new SqlCommand("DELETE FROM bands;", conn);
    cmd.ExecuteNonQuery();
    conn.Close();
  }



  }
}
