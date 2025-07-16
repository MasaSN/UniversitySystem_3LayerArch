using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Core.Exceptions
{
    public class NotFoundException :Exception
    {
        public NotFoundException() : base("Request Record not found") { }
        public NotFoundException(string message) : base(message) 
        {
        }
    }
}
