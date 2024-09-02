using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumApp.Models
{
    public class CommentViewModel
    {
        public int PostId { get; set; }
        [Required(ErrorMessage = "Comment content is required.")]
        public string Content { get; set; }
        public int? ParentCommentId { get; set; }  // Nullable to allow top-level comments
    }
}