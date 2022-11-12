using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace prs_serverside_2022.Models
{
    public class RequestLine
    {
        [Key]
        public int Id { get; set; } = 0;
        public int Quantity { get; set; } = 1;
        public int RequestId { get; set; } = 0;
        public int ProductId { get; set; } = 0;

        [JsonIgnore]
        public virtual Request? Request { get; set; } = null;
        public virtual Product? Product { get; set; } = null;

    }
}
