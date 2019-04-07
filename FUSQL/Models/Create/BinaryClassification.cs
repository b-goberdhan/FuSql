﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Models.Create
{
    public class BinaryClassification
    {
        public string Name { get; set; }
        public List<string> InputColumns { get; set; } = new List<string>();
        public string GoalColumn { get; set; }
        public string From { get; set; }
    }
}
