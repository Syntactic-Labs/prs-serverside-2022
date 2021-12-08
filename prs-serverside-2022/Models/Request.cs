using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prs_serverside_2022.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; } = 0;
        [Required, StringLength(80)]
        public string Description { get; set; } = string.Empty;
        [Required,StringLength(80)]
        public string Justification { get; set; } = string.Empty;
        [Required, StringLength (80)]
        public string? RejectionReason { get; set; } = null;
        [Required, StringLength(20)]
        public string DeliveryMode { get; set; } = "Pickup";
        [Required, StringLength(10)]
        public string Status { get; set; } = "New";
        [Required, Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; } = decimal.Zero;
        public int UserId { get; set; } = 0;

        public virtual User? User { get; set; } = default;

        public virtual IEnumerable<RequestLine>? Requestline { get; set; }
    }
}
