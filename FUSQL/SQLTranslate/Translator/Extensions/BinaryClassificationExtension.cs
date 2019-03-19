using Database.BaseDb;
using FUSQL.Mining;
using FUSQL.SQLTranslate.Results;
using System;
using System.Collections.Generic;

namespace FUSQL.SQLTranslate.Translator.Extensions
{
    public static class BinaryClassificationExtension
    {
        public static ResultView<TRowModel> RunBinaryClassification<TRowModel>(this Translation<TRowModel> translation, IDb db) where TRowModel : class, new()
        {
            if (translation.Operation.MiningOp != DataMinner.Mining.Enums.MiningOp.BinaryClassification)
            {
                throw new Exception("Cannot run binary classification on translation not meant for binary classification.");
            }

            // Gather initial data from the DB. We need this to train our clustering operation
            var sqlResults = new List<TRowModel>();
            translation.RunSQL(db, (model) =>
            {
                sqlResults.Add(model);
            });
            ResultView<TRowModel> resultView = new ClusterResultView<TRowModel>(translation.Operation.MiningOp);
            // Here we are doing a binary classification operation               
           // var binaryClassifier = new BinaryClassification<TRowModel>(sqlResults);
            // Build the model so we can begin to use and group up the data into specific clusters
           // binaryClassifier.BuildModel(translation.Operation.ClusterColumns.ToArray());
            int cluster = 1;
            // Prepare the clusters so data can be added into them
            while (cluster <= translation.Operation.ClusterCount)
            {
                (resultView as ClusterResultView<TRowModel>).Clusters[cluster] = new List<TRowModel>();
                cluster++;
            }
            // Iterate over the results and move models into the specified clusters
            sqlResults.ForEach((model) =>
            {
          //      int clusterId = Convert.ToInt32(binaryClassifier.Evaluate(model).SelectedClusterId);
          //      (resultView as ClusterResultView<TRowModel>).Clusters[clusterId].Add(model);
            });
            return resultView;
        }
    }
}
