using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShipmentMethodService.Models
{
    public class ShipmentMethod
    {
        [Required]
        public string Name { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ShipmentMethodId { get; set; }
        public string Description { get; set; }
    }
}
