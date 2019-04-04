using Database.BaseDb;
using DataMinner.Mining;
using System;
using System.Collections.Generic;

namespace FUSQL.SQLTranslate.Translator.Extensions
{
    public static class MulticlassClassificationExtension
    {
        public static MulticlassClassificationPrediction RunMulticlassClassification<TRowModel>(this Translation<TRowModel> translation, IDb db) where TRowModel : class, new()
        {
            if (translation.Operation.MiningOp != DataMinner.Mining.Enums.MiningOp.MultiClassification)
            {
                throw new Exception("Cannot run multiclass classification on translation not meant for multiclass classification.");
            }

            // Gather initial data from the DB. We need this to train our binary classification operation
            var sqlResults = new List<TRowModel>();
            translation.RunSQL(db, (model) =>
            {
                sqlResults.Add(model);
            });
    
            // Here we are doing a multiclass classification operation               
            var multiclassClassifier = new MulticlassClassification<TRowModel>(sqlResults);
            // Build the model so we can begin to use and classify
            multiclassClassifier.BuildModel();

            MulticlassClassificationData problem = new MulticlassClassificationData { Description = translation.Operation.Description };
            var result = multiclassClassifier.Evaluate(problem);
            return result;
        }
    }
}
