using System;
using System.Collections.Generic;

namespace ShauliProject.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string AuthorSite { get; set; }
        public DateTime PublishDate { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}