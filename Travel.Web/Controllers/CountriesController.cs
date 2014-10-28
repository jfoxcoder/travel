using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Travel.Web.DataContexts;
using Travel.Web.Enums;
using Travel.Web.Models;
using Travel.Web.ViewModels;

namespace Travel.Web.Controllers
{
    
    [Authorize(Roles="admin")]
    [RouteArea("Admin", AreaPrefix="admin")]
    [RoutePrefix("countries")]
    [Route("{action=Index}")]
    public class CountriesController : TravelControllerBase
    {
      

        // GET: Countries
        [Route("", Name="AdminCountriesHome")]
        public ActionResult Index()
        {
            LayoutVm.InitPage("Manage Countries", "View, edit, add and remove countries.");

            var countries = TravelDb.Countries.OrderBy(c => c.Name)
                              .Include(c => c.Destinations)
                              .ToList();

            var countryVms = countries.Select(c => new AdminCountryVm(c));

            return View(countryVms);
        }

        

       

        // GET: Countries/Create
        [Route("create", Name="CountryCreate")]
        public ActionResult Create()
        {
            LayoutVm.InitPage("Create Country");
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public ActionResult Create([Bind(Include = "Id,Name,IsoCode")] Country country)
        {
            EnsureNameAndIsoCodeAreNotInUse(country.Name, country.IsoCode);

            // need to do this to help protect against bugs that might occur
            // when dealing with third party components that require valid codes
            // e.g the flags icons.
            EnsureIsoCodeIsReal(country.IsoCode);
            
            if (ModelState.IsValid)
            {
                TravelDb.Countries.Add(country);
                TravelDb.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(country);
        }

        /// <summary>
        /// warning: The uniqueness of a country name or code needs to be case-insensitive.
        ///          This validation method uses linq-to-entities which will only produce
        ///          correct results if the database collation does case-insensitive comparisons.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isoCode"></param>
        /// <param name="modelState"></param>
        private void EnsureNameAndIsoCodeAreNotInUse(string name, string isoCode, int? id = null)
        {
            var countries = id.HasValue
                ? TravelDb.Countries.Where(c => c.Id != id.Value)
                : TravelDb.Countries;
                

            var duplicateName = countries.Any(c =>
                    c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (duplicateName)
            {
                ModelState.AddModelError("Name", string.Format("Country name '{0}' is in use", name));
                return;
            }

            var duplicateIsoCode = countries.Any(c =>
                    c.IsoCode.Equals(isoCode, StringComparison.InvariantCultureIgnoreCase));

            if (duplicateIsoCode)
            {
                ModelState.AddModelError("IsoCode", string.Format("Iso Code '{0}' is in use.", isoCode));
                return;
            }
        }

       

        /// <summary>
        /// Quick and dirty isocode validation
        /// 
        /// 
        /// </summary>
        /// <param name="isoCode"></param>
        private void EnsureIsoCodeIsReal(string isoCode)
        {
            var realIsoCodes = new string[] { "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AO", "AQ", "AR", "AS", "AT", "AU", "AW", "AX", "AZ", "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BL", "BM", "BN", "BO", "BQ", "BR", "BS", "BT", "BV", "BW", "BY", "BZ", "CA", "CC", "CD", "CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "CR", "CU", "CV", "CW", "CX", "CY", "CZ", "DE", "DJ", "DK", "DM", "DO", "DZ", "EC", "EE", "EG", "EH", "ER", "ES", "ET", "FI", "FJ", "FK", "FM", "FO", "FR", "GA", "GB", "GD", "GE", "GF", "GG", "GH", "GI", "GL", "GM", "GN", "GP", "GQ", "GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR", "HT", "HU", "ID", "IE", "IL", "IM", "IN", "IO", "IQ", "IR", "IS", "IT", "JE", "JM", "JO", "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW", "KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY", "MA", "MC", "MD", "ME", "MF", "MG", "MH", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ", "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU", "NZ", "OM", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PR", "PS", "PT", "PW", "PY", "QA", "RE", "RO", "RS", "RU", "RW", "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SR", "SS", "ST", "SV", "SX", "SY", "SZ", "TC", "TD", "TF", "TG", "TH", "TJ", "TK", "TL", "TM", "TN", "TO", "TR", "TT", "TV", "TW", "TZ", "UA", "UG", "UM", "US", "UY", "UZ", "VA", "VC", "VE", "VG", "VI", "VN", "VU", "WF", "WS", "YE", "YT", "ZA", "ZM", "ZW" };

            if (! realIsoCodes.Contains(isoCode.ToUpper()))
            {
                ModelState.AddModelError("IsoCode", string.Format("'{0}' is not a valid ISO-3166 Alpha-2 code.", isoCode));
            }
        }



        [Route("{id}/edit", Name = "CountryEditForm")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = TravelDb.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            LayoutVm.InitPage("Edit " + country.Name);
            return View(country);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id}/edit", Name="CountryEdit")]
        public ActionResult Edit([Bind(Include = "Id,Name,IsoCode")] Country country)
        {
            EnsureNameAndIsoCodeAreNotInUse(country.Name, country.IsoCode, country.Id);
            EnsureIsoCodeIsReal(country.IsoCode);

            ///* Kept getting exceptions when trying to pass the bound country parameter to
            // * TravelDb and I don't know why...
            // * Re-querying the context for the country works though... */
            var lookedUpCountry = TravelDb.Countries.Find(country.Id);
            lookedUpCountry.Name = country.Name;
            lookedUpCountry.IsoCode = country.IsoCode;

            if (ModelState.IsValid)
            {
                TravelDb.Entry(lookedUpCountry).State = EntityState.Modified;
                TravelDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(country);
        }

        


        [Route("{id}/delete", Name = "CountryDelete")]        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = TravelDb.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            LayoutVm.InitPage("Delete " + country.Name);
            return View(country);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("{id}/delete")]        
        public ActionResult DeleteConfirmed(int id)
        {
            Country country = TravelDb.Countries.Find(id);
            TravelDb.Countries.Remove(country);
            TravelDb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TravelDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
