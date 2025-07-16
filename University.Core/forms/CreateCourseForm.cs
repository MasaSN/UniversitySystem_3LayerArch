using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Core.forms
{
    public class CreateCourseForm
    {
        [Required]
        public string CourseName { get; set; }
        public string description { get; set; }
    }
}
