using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumApp.Models
{
    // Models/PostVote.cs
    public class PostVote
    {
        [Key]
        public int PostVoteId { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        // 1 for upvote, -1 for downvote
        public int VoteType { get; set; }

        public DateTime DateVoted { get; set; }
    }

}