using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CollectingIdeas.Models
{
    public class Idea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public string? Path { get; set; }
        public DateTime CreateDatetime { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public virtual Category Category { get; set; }
        [Required]
        [ForeignKey("TopicId")]
        public int TopicId { get; set; }
        [ValidateNever]
        public virtual Topic Topic { get; set; }
        [ForeignKey("IdentityUserId")]
        [ValidateNever]
        public string IdentityUserId { get; set; }
        [ValidateNever]
        public virtual IdentityUser IdentityUser { get; set; }
    }
}
