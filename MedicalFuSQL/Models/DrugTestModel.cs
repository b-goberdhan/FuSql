﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalFuSQL.Models
{
    public class DrugTestModel
    {
        public long field1 { get; set; }
        public long field { get; set; }
        public string urlDrugName { get; set; }
        public float rating { get; set; }
        public string effectiveness { get; set; }
        public string condition { get; set; }
        public string sideEffectsReview { get; set; }
        public string sideEffects { get; set; }
        public string commentsReview { get; set; }
        public bool ratingEnum { get; set; }

        public override string ToString()
        {
            return "Key: " + field;
        }
    }
}
