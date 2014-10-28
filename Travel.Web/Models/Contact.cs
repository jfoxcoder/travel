using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Travel.Web.Models
{

    /// <summary>
    /// Represents a contact message received through the contact us form.
    /// </summary>
    public class Contact
    {        
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]        
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 30 characters.")]
        [MaxLength(30)]        
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 30 characters.")]
        [MaxLength(30)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone")]        
        [MaxLength(30)]
        [StringLength(30, MinimumLength = 7, ErrorMessage = "Phone number must be between 7 and 30 characters.")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(2000)] // For EF
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 2000 characters.")]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}