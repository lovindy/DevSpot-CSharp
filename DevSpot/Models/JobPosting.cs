using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Models
{
    public class JobPosting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string Company { get; set; }
        [Required]
        public required string Location { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public bool IsApproved { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}

