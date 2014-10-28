using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Travel.Web.ExtensionMethods;

namespace Travel.Web.Models
{
    public class Country// : IPropertyDerivedFileDirectory
    {
        private string name;

        private ICollection<Destination> destinations;

        public Country()
        {
            destinations = new List<Destination>();
        }

        [Key]
        public int Id { get; set; }

      
        /// <summary>
        /// IS0 3166 Alpha2 Country code, e.g., New Zealand = NZ
        /// 
        /// Currently only used as css flag selector (was used in image paths)
        /// 
        /// </summary>
        [Required]
        [Display(Name = "ISO-3166 Alpha-2 Country Code")]
        [StringLength(2, MinimumLength = 2, ErrorMessage="ISO-3166 Alpha 2 codes have exactly two letters.")]
        [Index("IsoCode", 1, IsUnique=true)]
        public string IsoCode { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(60, MinimumLength = 4, ErrorMessage = "Country name must be between 4 and 60 characters.")]
        [MaxLength(60)]
        [Index("Name", 2, IsUnique = true)]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                Slug = value.ToSlug();
            }
        }

        public virtual ICollection<Destination> Destinations
        {
            get
            {
                return destinations;
            }
            set
            {
                destinations = value;
            }
        }

        /// <summary>
        /// Used to generate SEO-friendly, readable URLs and image directories.
        /// 
        /// Set via Name property.
        /// </summary>
        [Required]
        [Display(Name = "Slug")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Slug name must be between 4 and 100 characters.")]
        [MaxLength(100)]
        [Index("Slug", 4, IsUnique = true)]        
        public string Slug {get; set;}

       
    }
}