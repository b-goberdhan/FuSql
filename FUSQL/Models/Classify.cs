using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Models
{
    public class Classify
    {
        public List<Term> Terms { get; set; } = new List<Term>();
        public string ClassifierName { get; set; }
        
    }
}
