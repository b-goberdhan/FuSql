using FUSQL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Models
{
    public class Command
    {
        public Create.Create Create { get; set; }
        public Delete Delete { get; set; }
        public Classify Classify { get; set; }
        
        public Find Find { get; set; }
        public Get Get { get; set; } = Get.None;
        public Check Check { get; set; }
        public Identify Identify { get; set; }
    }
}
