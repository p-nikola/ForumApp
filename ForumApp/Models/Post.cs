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
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public string UserId { get; set; }

        [ForeignKey("Forum")]
        public int ForumId { get; set; }
        public virtual Forum Forum { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int Upvotes => Votes?.Where(v => v.VoteType == 1).Count() ?? 0;
        public int Downvotes => Votes?.Where(v => v.VoteType == -1).Count() ?? 0;

        public bool IsApproved { get; set; } = false;

        public virtual ICollection<PostVote> Votes { get; set; }


        public Post()
        {
            Comments = new HashSet<Comment>();
            Votes = new HashSet<PostVote>();    
        }


    }
}