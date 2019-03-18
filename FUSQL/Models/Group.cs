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
        public int Count { get; set; }
        public List<string> Columns { get; set; } = new List<string>();
    }
}
