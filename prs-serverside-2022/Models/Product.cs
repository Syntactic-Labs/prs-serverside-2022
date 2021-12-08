using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prs_serverside_2022.Models
{
    [Index("PartNbr", IsUnique = true)]
    public class Product
    {
        [Key]
        public int Id { get; set; } = 0;
        [StringLength(30), Required]
        public string ?PartNbr { get; set; } = string.Empty;
        [StringLength(30), Required]
        public string? Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(11,2)"), Required]
        public decimal Price { get; set; } = decimal.Zero;
        [StringLength(30), Required]
        public string? Unit { get; set; } = "Each";
        [StringLength(30)]
        public string? PhotoPath { get; set; } = null;
        public int VendorId { get; set; } = 0;

        public virtual Vendor? Vendor { get; set; }
        [JsonIgnore]
        public virtual ICollection<RequestLine>? Requestline { get; set; }

        
    }
}
