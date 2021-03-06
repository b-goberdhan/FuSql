﻿using DataMinner.Mining.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public class ClusterResultView<TModel> : ResultView<TModel> where TModel: class, new()
    {
        public Dictionary<int, List<TModel>> Clusters { get; private set; }
        public ClusterResultView(MiningOp miningOp) : base(miningOp)
        {
            Clusters = new Dictionary<int, List<TModel>>();
        }

    }
}
