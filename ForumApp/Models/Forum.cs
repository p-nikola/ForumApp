using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace ForumApp.Models
{
    public class Forum
    {
        public int ForumId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public Forum()
        {
            Posts = new HashSet<Post>();
        }


    }
}