using System.ComponentModel.DataAnnotations;

namespace WebCollectingIdeas.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreateDatetime { get; set; } = DateTime.Now;
    }
}
