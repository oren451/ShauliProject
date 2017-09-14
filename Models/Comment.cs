using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShauliProject.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public string Title { get; set; }
        public string CommentWriter { get; set; }
        public string CommentWriterSite { get; set; }
        public string Content { get; set; }
        public virtual Post Post { get; set; }



    }
}