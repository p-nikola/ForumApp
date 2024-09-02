using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumApp.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public string UserId { get; set; }

        [ForeignKey("Forum")]
        public int ForumId { get; set; }
        public virtual Forum Forum { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public Post()
        {
            Comments = new HashSet<Comment>();
        }


    }
}