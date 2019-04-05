using Database.BaseDb;
using DataMinner.Mining;
using System;
using System.Collections.Generic;

namespace FUSQL.SQLTranslate.Translator.Extensions
{
    public static class BinaryClassificationExtension
    {
        public static BinaryClassificationPrediction RunBinaryClassification<TRowModel>(this Translation<TRowModel> translation, IDb db) where TRowModel : class, new()
        {
            if (translation.Operation.MiningOp != DataMinner.Mining.Enums.MiningOp.BinaryClassification)
            {
                throw new Exception("Cannot run binary classification on translation not meant for binary classification.");
            }

            // Gather initial data from the DB. We need this to train our binary classification operation
            var sqlResults = new List<TRowModel>();
            translation.RunSQL(db, (model) =>
            {
                sqlResults.Add(model);
            });
            
            // Here we are doing a binary classification operation               
            var binaryClassifier = new BinaryClassification<TRowModel>(sqlResults);
            // Build the model so we can begin to use and classify
            binaryClassifier.BuildModel();

            BinaryClassificationData sentiment = new BinaryClassificationData { commentsReview = translation.Operation.Description};
            var result = binaryClassifier.Evaluate(sentiment);
            return result;
        }
    }
}
