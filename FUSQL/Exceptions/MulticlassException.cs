using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Exceptions
{
    public class MultiClassException : Exception
    {
        public string ErrorMessage { get; set; }
    }
}
