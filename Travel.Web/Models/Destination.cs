using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Travel.Web.Models;
using Travel.Web.ExtensionMethods;
using System.ComponentModel;

namespace Travel.Web.Models
{
    public class Destination
    {
        private string name;
        private string description;
        private int featured;

       // private ICollection<ApplicationUser> applicationUsers;

        public Destination()
        {
           // applicationUsers = new List<ApplicationUser>();
        }
       


        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(60, MinimumLength = 4, ErrorMessage = "Destination name must be between 4 and 60 characters.")]
        [MaxLength(60)]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                Slug = value.ToSlug();
            }
        }

        [Required]
        [Display(Name = "Description")]
        [StringLength(8000, MinimumLength = 8, ErrorMessage = "Description must be between 8 and 8000 characters.")]
        [MaxLength(8000)]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value.Trim();
            }
        }

        
        public virtual Country Country { get; set; }

        [Required]
        [Display(Name="Country")]
        public virtual int CountryId { get; set; }


        /// <summary>
        /// Used to generate SEO-friendly, readable URLs and image directories.
        /// 
        /// Set via Name property.
        [Required]
        [Display(Name = "Slug")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Slug name must be between 4 and 100 characters.")]
        [MaxLength(100)]       
        public string Slug { get; set; }



        [Required]
        [Display(Name = "Featured")]                
        [Description("Allows destinations to be promoted, eg Featured sorting etc.")]
        public int Featured
        {
            get
            {
                return featured;
            }
            set
            {
                featured = value;
            }
        }
    
    }
}