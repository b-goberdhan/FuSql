using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Models
{
    public class Check
    {
        public List<Term> Terms { get; set; }
        public string BinaryClassifierName { get; set; }
    }
}
