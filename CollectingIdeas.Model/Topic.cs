using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollectingIdeas.Models
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreateDatetime { get; set; } = DateTime.Now;
        public DateTime ClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }
    }
}
