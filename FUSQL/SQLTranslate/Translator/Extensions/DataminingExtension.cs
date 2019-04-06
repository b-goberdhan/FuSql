using Database.BaseDb;
using DataMinner.Mining.Enums;
using FUSQL.InternalModels;
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
                case MiningOp.Classify:
                    return translation.RunClassifier();
                case MiningOp.BuildMultiClassification:
                    CreateMultiClassifier<TRowModel>(translation, db);
                    return null;
                case MiningOp.None:
                    return null;
                default:
                    return null;
            };
        }
        private static void CreateMultiClassifier<TRowModel>(Translation<TRowModel> translation, IDb db) where TRowModel : class, new()
        {
            var operation = translation.Operation as BuildClassificationOperation;
            var classifier = translation.BuildMulticlassClassification(db);
            FusqlInternal<TRowModel>.GetInstance().AddMultiClassifier(operation.Name, classifier);
        }
    }
}
