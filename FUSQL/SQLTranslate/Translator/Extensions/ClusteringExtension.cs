using Database.BaseDb;
using FUSQL.Mining;
using FUSQL.SQLTranslate.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Translator.Extensions
{
    public static class ClusteringExtension
    {
        public static ResultView<TRowModel> RunClustering<TRowModel>(this Translation<TRowModel> translation, IDb db) where TRowModel : class, new()
        {
            if (translation.Operation.MiningOp != DataMinner.Mining.Enums.MiningOp.Clustering)
            {
                throw new Exception("Cannot run clustering on translation not meant for clustering.");
            }

            var sqlResults = new List<TRowModel>();
            //Gather initial data, we need this so we can train our clustering
            translation.RunSQL(db, (model) =>
            {
                sqlResults.Add(model);
            });          
            ResultView<TRowModel> resultView = new ClusterResultView<TRowModel>(translation.Operation.MiningOp);
            // Here we are doing a clustering operation               
            var clusterer = new Clustering<TRowModel>(translation.Operation.ClusterCount, sqlResults);
            // Build the model so we can being to use and group up the data into specific clusters
            clusterer.BuildModel(translation.Operation.ClusterColumns.ToArray());
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
                int clusterId = Convert.ToInt32(clusterer.Evaluate(model).SelectedClusterId);
                (resultView as ClusterResultView<TRowModel>).Clusters[clusterId].Add(model);
            });
            return resultView;
        }
        
    }
}
