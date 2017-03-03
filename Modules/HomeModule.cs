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





        }

    }

}
