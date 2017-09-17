using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShauliProject.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        [Display(Name = "Comment Title")]
        public string Title { get; set; }
        [Display(Name = "Comment Writer")]
        public string CommentWriter { get; set; }
        [Display(Name = "Writer URL")]
        public string CommentWriterSite { get; set; }
        public string Content { get; set; }
        public virtual Post Post { get; set; }



    }
}