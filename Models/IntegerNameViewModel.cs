using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShauliProject.Models
{
    public class IntegerNameViewModel
    {
        public IntegerNameViewModel(int i, string n)
        {
            integer = i;
            name = n;
        }

        public int integer { get; set; }
        public string name { get; set; }
    }
}