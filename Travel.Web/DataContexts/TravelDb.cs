using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Travel.Web.Models;
using Travel.Web.Utilities;
using Travel.Web.ViewModels;

namespace Travel.Web.DataContexts
{
    public class TravelDb : DbContext
    {
        public TravelDb()
            : base("DefaultConnection")
        {           
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Destination> Destinations { get; set; }

        public DbSet<Contact> Contacts { get; set; }



     

        


        public IEnumerable<DbEntityEntry<Destination>> AddedDestinationEntries
        {
            get
            {
                return ChangeTracker.Entries<Destination>()
                                    .Where(ce => ce.State == EntityState.Added);
            }
        }

        public IEnumerable<DbEntityEntry<Destination>> DestinationsWithModifiedSlug
        {
            get
            {
                return ChangeTracker.Entries<Destination>()
                                    .Where(ce => ce.Property(c => c.Slug).IsModified);
            }
        }

        public IEnumerable<DbEntityEntry<Country>> CountriesWithModifiedSlug
        {
            get
            {
                return ChangeTracker.Entries<Country>()
                                    .Where(ce => ce.Property(c => c.Slug).IsModified);
            }
        }


        public override int SaveChanges()
        {            
            var imagePathTool = new ImagePathUtil();

            // New Destinations

            // Add image directories
            foreach (var entry in AddedDestinationEntries)
            {                
                imagePathTool.CreateDirectories(
                    countrySlug: entry.Entity.Country.Slug,
                    destinationSlug: entry.Entity.Slug);
            }

            // Modified Destination Name/Slug

            // Rename image destinations
            foreach (var entry in DestinationsWithModifiedSlug)
            {                
                imagePathTool.RenameDestinationDirectories(
                    countrySlug: entry.Entity.Country.Slug,
                    originalDestinationSlug: entry.OriginalValues.GetValue<string>("Slug"),
                    currentDestinationSlug: entry.Entity.Slug);
            }

            // Modified Country Name

            // Rename country directory
            foreach (var entry in CountriesWithModifiedSlug)
            {
                imagePathTool.RenameCountryDirectories(                    
                    originalCountrySlug: entry.OriginalValues.GetValue<string>("Slug"),
                    currentCountrySlug: entry.Entity.Slug);
            }

            return base.SaveChanges();
        }







    }
}