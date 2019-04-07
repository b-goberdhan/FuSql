using Database.BaseDb;
using DataMinner.Mining;
using DataMinner.Mining.BinaryClassification;
using FUSQL.SQLTranslate.Results;
using System;
using System.Collections.Generic;

namespace FUSQL.SQLTranslate.Translator.Extensions
{
    public static class BinaryClassificationExtension
    {
        public static ResultView RunBinaryClassification<TRowModel>(this Translation<TRowModel> translation, IDb db) where TRowModel : class, new()
        {

            var operation = translation.Operation as BuildBinaryClassificationOperation;
            // Gather initial data from the DB. We need this to train our binary classification operation
            var sqlResults = new List<TRowModel>();
            translation.RunSQL(db, (model) =>
            {
                sqlResults.Add(model);
            });

            // Here we are doing a binary classification operation               
            var binaryClassifier = new BinaryClassification<TRowModel>(sqlResults);
            // Build the model so we can begin to use and classify
            binaryClassifier.CreateBuild(operation.Goal, operation.InputColumns.ToArray());
            return new BuildClassifierResultView()
            {
                Name = operation.Name
            };

        }
    }
}
