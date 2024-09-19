using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace ForumApp.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public string UserId { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        // Self-referencing foreign key for replies
        public int? ParentCommentId { get; set; }
        [ForeignKey("ParentCommentId")]
        public virtual Comment ParentComment { get; set; }

        // Collection to hold the replies to this comment
        public virtual ICollection<Comment> Replies { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int Upvotes { get; set; }
        public int Downvotes { get; set; }


        public Comment()
        {
            Replies = new HashSet<Comment>();
        }


    }
}