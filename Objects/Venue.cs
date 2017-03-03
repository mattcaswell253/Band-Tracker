using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace BandTracker
{
    public class Venue
    {
      private int _id;
      private string _name;

      public Venue(string Name, int Id = 0)
      {
         _id = Id;
         _name = Name;
      }

      public override bool Equals(System.Object otherVenue)
        {
            if (!(otherVenue is Venue))
            {
                return false;
            }
            else
            {
                Venue newVenue = (Venue) otherVenue;
                bool idEquality = this.GetId() == newVenue.GetId();
                bool nameEquality = this.GetName() == newVenue.GetName();
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
        public static List<Venue> GetAll()
        {
            List<Venue> allVenues = new List<Venue>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM venues;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int VenueId = rdr.GetInt32(0);
                string VenueName = rdr.GetString(1);
                Venue newVenue = new Venue(VenueName, VenueId);
                allVenues.Add(newVenue);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return allVenues;
        }

        public void Save()
      {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("INSERT INTO venues (name) OUTPUT INSERTED.id VALUES (@VenueName);", conn);

          SqlParameter nameParameter = new SqlParameter();
          nameParameter.ParameterName = "@VenueName";
          nameParameter.Value = this.GetName();
          cmd.Parameters.Add(nameParameter);
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

      public static Venue Find(int id)
      {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("SELECT * FROM venues WHERE id = @VenueId;", conn);

          cmd.Parameters.Add(new SqlParameter("@VenueId", id.ToString()));

          SqlDataReader rdr = cmd.ExecuteReader();

          int foundVenueId = 0;
          string foundVenueName = null;

          while(rdr.Read())
          {
              foundVenueId = rdr.GetInt32(0);
              foundVenueName = rdr.GetString(1);
          }
          Venue foundVenue = new Venue(foundVenueName, foundVenueId);

          if (rdr != null)
          {
              rdr.Close();
          }
          if (conn != null)
          {
              conn.Close();
          }
          return foundVenue;
      }

    }
}
