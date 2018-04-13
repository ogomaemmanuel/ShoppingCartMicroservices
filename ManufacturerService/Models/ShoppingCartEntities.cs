using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ManufacturerService.Models
{
    public class Manufacturer
    {
        [Required]
        public string ManufacturerName { get; set; }
        public string ManufacturerEmail { get; set; }
        public string ManufacturerCategory { get; set; }
        public string ManufacturerUrl { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ManufacturerId { get; set; }
    }
    public class ManufacturerCategory
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }
    }
}
