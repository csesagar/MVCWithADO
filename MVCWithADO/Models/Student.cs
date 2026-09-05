using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCWithADO.Models
{
    public class Student
    {
        [Display(Name = "Student Id")]
        public int Sid { get; set; }
        public string Name { get; set; }
        public int? Class { get; set; }
        public decimal? Fees { get; set; }
        public string Photo { get; set; }
    }
}