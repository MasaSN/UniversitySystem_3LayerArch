using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace University.Core.forms
{
    public class CreateStudentFrom 
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        
        public string Email { get; set; }

        
    }
}
