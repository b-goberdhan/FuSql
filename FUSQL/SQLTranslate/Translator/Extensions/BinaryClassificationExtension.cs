using Database.BaseDb;
using DataMinner.Mining;
using DataMinner.Mining.BinaryClassification;
using FUSQL.InternalModels;
using FUSQL.SQLTranslate.Results;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FUSQL.SQLTranslate.Translator.Extensions
{
    public static class BinaryClassificationExtension
    {
        public static ResultView BuildBinaryClassification<TRowModel>(this Translation<TRowModel> translation, IDb db) where TRowModel : class, new()
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
            FusqlInternal<TRowModel>.GetInstance().AddBinaryClassifier(operation.Name, binaryClassifier);
            return new BuildMultiClassifierResultView()
            {
                Name = operation.Name
            };

        }
        public static ResultView RunBinaryClassifier<TRowModel>(this Translation<TRowModel> translation) where TRowModel : class, new()
        {
            var operation = translation.Operation as RunBinaryClassificationOperation;
            var classifier = FusqlInternal<TRowModel>.GetInstance().GetBinaryClassification(operation.BinaryClassifierName);

            TRowModel data = Activator.CreateInstance(typeof(TRowModel)) as TRowModel;
            foreach (var term in operation.Terms)
            {
                PropertyInfo info = data.GetType().GetProperty(term.Column);
                info.SetValue(data, term.Value);
            }
            var prediction = classifier.Evaluate(data);
            return new RunBinaryClassifierResultView()
            {
                Probability = prediction.Probability,
                Prediction = prediction.Prediction
            };
        }
        public static ResultView DeleteBinaryClassifer<TRowModel>(this Translation<TRowModel> translation) where TRowModel : class, new()
        {
            var operation = translation.Operation as DeleteBinaryClassificationOperation;
            FusqlInternal<TRowModel>.GetInstance().DeleteBinaryClassifier(operation.Name);
            return new DeleteBinaryClassifierResultView()
            {
                Name = operation.Name
            };
        }

    }
}
