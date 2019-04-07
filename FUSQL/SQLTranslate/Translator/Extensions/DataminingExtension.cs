using Database.BaseDb;
using DataMinner.Mining.Enums;
using FUSQL.InternalModels;
using FUSQL.SQLTranslate.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Translator.Extensions
{
    public static class DataMiningExtension
    {
        public static IResultView RunDataMining<TRowModel>(this Translation<TRowModel> translation, IDb db) where TRowModel : class, new() 
        {
            switch(translation.Operation.MiningOp)
            {
                case MiningOp.Clustering:
                    return translation.RunClustering(db);
                //case MiningOp.BinaryClassification:
                  //  return translation.RunBinaryClassification(db);
                case MiningOp.Classify:
                    return translation.RunClassifier();
                case MiningOp.BuildBinaryClassification:
                    return translation.RunBinaryClassification(db);
                case MiningOp.BuildMultiClassification:
                    return translation.BuildMulticlassClassification(db);
                case MiningOp.DeleteMultiClassification:
                    return translation.DeleteClassifier();
                case MiningOp.None:
                    return null;
                default:
                    return null;
            };
        }
    }
}
