using System.Collections.Generic;
using ShauliProject.Models;

namespace ShauliProject.Models
{
    public class PostUserViewModel
    {
        public Post post { get; set; }
        public ApplicationUser user { get; set; }
    }
}