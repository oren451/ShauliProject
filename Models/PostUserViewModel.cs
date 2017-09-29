using System.Collections.Generic;
using ShauliProject.Models;

namespace ShauliProject.Models
{
    public class PostUserViewModel
    {
        public List<Post> postlist { get; set; }
        public List<ApplicationUser> userlist { get; set; }
    }
}