using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumApp.Models
{
    // Models/CommentVote.cs
    public class CommentVote
    {
        [Key]
        public int CommentVoteId { get; set; }

        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        // 1 for upvote, -1 for downvote
        public int VoteType { get; set; }

        public DateTime DateVoted { get; set; }
    }

}