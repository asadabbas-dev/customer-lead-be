using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerLeadApi.Models
{
    public class CustomerImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string ImageData { get; set; } = string.Empty; // Base64 encoded

        [MaxLength(100)]
        public string? FileName { get; set; }

        [MaxLength(50)]
        public string? ContentType { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;
    }
}