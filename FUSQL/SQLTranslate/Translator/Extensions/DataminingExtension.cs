using Database.BaseDb;
using DataMinner.Mining.Enums;
using FUSQL.SQLTranslate.Results;

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
                    return translation.RunMultiClassifier();
                case MiningOp.Check:
                    return translation.RunBinaryClassifier();
                case MiningOp.BuildBinaryClassification:
                    return translation.BuildBinaryClassification(db);
                case MiningOp.BuildMultiClassification:
                    return translation.BuildMultiClassification(db);
                case MiningOp.DeleteMultiClassification:
                    return translation.DeleteMultiClassifier();
                case MiningOp.DeleteBinaryClassification:
                    return translation.DeleteBinaryClassifer();
                case MiningOp.None:
                    return null;
                default:
                    return null;
            };
        }
    }
}
