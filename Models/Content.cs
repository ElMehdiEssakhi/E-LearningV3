using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_LearningV3.Models
{
    public class Content
    {
        public enum ContentType { Video, Pdf, Text }

        [Key]
        public int ContentId { get; set; }

        [Required]
        public ContentType Type { get; set; }

        [Required]
        public string ContentLink { get; set; } = null!; // URL to file or direct text

        // Foreign Key to Chapter
        public int ChapId { get; set; }
        [ForeignKey("ChapId")]
        public Chapter Chapter { get; set; } = null!;
    }
}
