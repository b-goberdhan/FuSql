using Database.BaseDb;
using DataMinner.Mining.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Translator.Extensions
{
    public static class DataMiningExtension
    {
        public static object RunDataMining<TRowModel>(this Translation<TRowModel> translation, IDb db) where TRowModel : class, new() 
        {
            switch(translation.Operation.MiningOp)
            {
                case MiningOp.Clustering:
                    return translation.RunClustering(db);
                case MiningOp.BinaryClassification:
                    return translation.RunBinaryClassification(db);
                case MiningOp.MultiClassification:
                    return translation.RunMulticlassClassification(db);
                case MiningOp.BuildMultiClassification:
                    return translation.BuildMulticlassClassification(db);
                case MiningOp.None:
                    return null;
                default:
                    return null;
            };
        }
    }
}
