using Database.BaseDb;
using DataMinner.Mining.MultiClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Translator.Extensions
{
    public static class MultiClassificationExtension
    {
        public static MultiClassification<TRowModel> BuildMulticlassClassification<TRowModel>(this Translation<TRowModel> translation, IDb db) where TRowModel : class, new()
        {
            if (translation.Operation.MiningOp != DataMinner.Mining.Enums.MiningOp.BuildMultiClassification)
            {
                throw new Exception("Cannot run multiclass classification on translation not meant for multiclass classification.");
            }
            var operation = translation.Operation as BuildClassificationOperation;
            // Gather initial data from the DB. We need this to train our binary classification operation
            var sqlResults = new List<TRowModel>();
            translation.RunSQL(db, (model) =>
            {
                sqlResults.Add(model);
            });

            // Here we are doing a multiclass classification operation               
            var multiclassClassifier = new MultiClassification<TRowModel>(sqlResults);
            // Build the model so we can begin to use and classify
            multiclassClassifier.CreateBuild(operation.Goal, operation.InputColumns.ToArray());

            // MulticlassClassificationData problem = new MulticlassClassificationData {
            //    Description = translation.Operation.Description };

            return multiclassClassifier;
        }
    }
}
