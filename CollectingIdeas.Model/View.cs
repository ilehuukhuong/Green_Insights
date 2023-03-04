using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingIdeas.Models
{
    public class View
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastVisit { get; set; }
        [Required]
        public int? IdeaId { get; set; }
        [Required]
        public string IdentityUserId { get; set; }

    }
}
