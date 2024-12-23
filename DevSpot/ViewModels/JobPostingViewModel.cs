using System.ComponentModel.DataAnnotations;

namespace DevSpot.ViewModels
{
    public class JobPostingViewModel
    {
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string Company { get; set; }
        [Required]
        public required string Location { get; set; }
    }
}
