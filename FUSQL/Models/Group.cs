using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Models
{
    public class Group
    {
        public string Name { get; set; }
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();
    }
}
