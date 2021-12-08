using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace prs_serverside_2022.Models
{
    [Index("Username", IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        [StringLength(30), Required]
        public string Username { get; set; } = "";
        [StringLength(30), Required]
        public string Password { get; set; } = string.Empty;
        [StringLength(30), Required]
        public string Firstname { get; set; } = string.Empty;
        [StringLength(30), Required]
        public string Lastname { get; set; } = string.Empty;
        [StringLength(12)]
        public string ?Phone { get; set; } = null;
        [StringLength(255)]
        public string ?Email { get; set; } = null;
        [Required]
        public bool IsReviewer { get; set; } = false;
        [Required]
        public bool IsAdmin { get; set; } = false;
    }
}
