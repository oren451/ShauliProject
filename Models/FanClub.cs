using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShauliProject.Models
{
    public class FanClub
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Seniority { get; set; }
        public string Address { get; set; }
    }



}