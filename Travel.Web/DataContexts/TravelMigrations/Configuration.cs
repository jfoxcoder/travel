using System.Collections.Generic;

namespace Travel.Web.DataContexts.TravelMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Xml.Linq;
    using Travel.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Travel.Web.DataContexts.TravelDb>
    {
        private TravelDb context;
        XDocument xml;    
        public Configuration()
        {
          

            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"DataContexts\TravelMigrations";

          
            // Load the XML seed data            
            var resourceName = "Travel.Web.App_Data.TravelSeedData.xml";
            var assembly = Assembly.GetExecutingAssembly();            
            var stream = assembly.GetManifestResourceStream(resourceName);
            xml = XDocument.Load(stream);     
        }

        

        protected override void Seed(TravelDb context)
        {
            // set the context
            this.context = context;



            //DeleteContactMessages();
            context.Contacts.RemoveRange(context.Contacts);
            context.SaveChanges();
            context.Countries.RemoveRange(context.Countries);
            context.SaveChanges();

            // create countries
            

            SeedCountries();
            context.SaveChanges();


            // create destinations
            SeedDestinations();

            context.SaveChanges();
        }

        private void DeleteContactMessages()
        {
            context.Contacts.RemoveRange(context.Contacts);
        }

        private void DeleteCountriesAndDestinations()
        {
         //   context.Countries.emp
           // context.Countries.RemoveRange(context.Countries);
        }

        private void SeedCountries()
        {
            var countries = from c in xml.Descendants("Country")
                            select new Country
                            {
                                Name = c.Attribute("Name").Value,
                                Slug = c.Attribute("Slug").Value,
                                IsoCode = c.Attribute("IsoCode").Value,
                            };

            context.Countries.AddOrUpdate(c => c.Name, countries.ToArray());

           
        }

        private void SeedDestinations()
        {
            
            var dests = new List<Destination>();

            foreach (var countryEl in xml.Descendants("Country"))
            {
                var countryName = countryEl.Attribute("Name").Value;
                var country = context.Countries
                                     .Single(c => c.Name == countryName);

                foreach (var destEl in countryEl.Descendants("Destination"))
                {
                    var destName = destEl.Attribute("Name").Value;
                    var dest = new Destination
                    {
                        Slug = destEl.Attribute("Slug").Value,
                        Description = destEl.Element("Description").Value,
                        Featured = (int)destEl.Attribute("Featured"),

                        Name = destName,                        
                        Country = country,
                        CountryId = country.Id
                    };
                    dests.Add(dest);
                }
            }

            context.Destinations.AddOrUpdate(d => d.Name, dests.ToArray());            
        }
    }
}
