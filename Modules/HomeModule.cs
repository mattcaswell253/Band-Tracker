using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace BandTracker
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                List<Venue> allVenues = Venue.GetAll();
                return View["index.cshtml", allVenues];
              };

            Get["/band/new"] = _ => {
                return View["bands_venues.cshtml"];
            };

            Post["/"] = _ => {
              Venue newVenue = new Venue(Request.Form["venue-name"]);
              newVenue.Save();
              return View["index.cshtml", Venue.GetAll()];
            };

            Post["/venue/{id}/band/new"] = parameters => {
              Band newBand = new Band(Request.Form["band-name"]);
              newBand.Save();
              Venue SelectedVenue = Venue.Find(parameters.id);
              SelectedVenue.AddBand(newBand);
              Dictionary<string, object> model = new Dictionary<string, object>();
              List<Band> BandList = SelectedVenue.GetBands();
              model.Add("venue", SelectedVenue);
              model.Add("bands", BandList);

              return View["bands_venues.cshtml", model];
          };

          Get["/venue/{id}"] = parameters => {
              Dictionary<string, object> model = new Dictionary<string, object>();
              Venue SelectedVenue = Venue.Find(parameters.id);
              List<Band> BandList = SelectedVenue.GetBands();
              model.Add("venue", SelectedVenue);
              model.Add("bands", BandList);
              return View["bands_venues.cshtml", model];
          };


          Post["/band/{id}/venue"] = parameters => {
            Venue newVenue = new Venue(Request.Form["venue-name"]);
            newVenue.Save();
            Band SelectedBand = Band.Find(parameters.id);
            SelectedBand.AddVenue(newVenue);
            Dictionary<string, object> model = new Dictionary<string, object>();
            List<Venue> VenueList = SelectedBand.GetVenues();
            model.Add("band", SelectedBand);
            model.Add("venue", VenueList);

            return View["venues_of_bands.cshtml", VenueList];
          };

          Get["/band/{id}"] = parameters => {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Band SelectedBand = Band.Find(parameters.id);
            List<Venue> VenueList = SelectedBand.GetVenues();
            model.Add("band", SelectedBand);
            model.Add("venue", VenueList);
            return View["venues_of_bands.cshtml", model];
          };

          Delete["/venue/delete/{id}"] = parameters => {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Venue SelectedVenue = Venue.Find(parameters.id);
            SelectedVenue.Delete();
            model.Add("venue", SelectedVenue);
            return View["success.cshtml", model];
          };

          Get["/band/delete/{id}"] = parameters => {
            Band SelectedBand = Band.Find(parameters.id);
            return View["bands_venues.cshtml", SelectedBand];
          };

          Patch["/venue/edit/{id}"] = parameters => {
            Venue SelectedVenue = Venue.Find(parameters.id);
            SelectedVenue.UpdateVenue(Request.Form["venue-name"]);
            return View["index.cshtml", Venue.GetAll()];
          };




          Delete["/band/delete/{id}"] = parameters => {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Band SelectedBand = Band.Find(parameters.id);
            SelectedBand.Delete();
            model.Add("band", SelectedBand);
            return View["success.cshtml", model];
          };

        }

    }

}
