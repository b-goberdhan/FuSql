﻿using FUSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate
{
    public class RunClassificationOperation : Operation
    {
        public List<Term> Terms { get; set; }
        public string ClassifierName { get; set; }

        public RunClassificationOperation() : base(null, DataMinner.Mining.Enums.MiningOp.Classify) { }
    }
}