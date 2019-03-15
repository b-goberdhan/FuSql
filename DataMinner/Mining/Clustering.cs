using DataMinner.Mining;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Mining
{
    public class Clustering<TRowModel> where TRowModel : class
    {
        private readonly MLContext _mlContext;
        private IDataView _dataView;
        private PredictionEngine<TRowModel, ClusterPrediction> _predictions;

        public Clustering(int clusterCount, IEnumerable<TRowModel> rows)
        {
            _mlContext = new MLContext();
            _dataView = _mlContext.Data.LoadFromEnumerable<TRowModel>(rows);
        }

        // Prepare a pipeline for training, train it, and create a prediction object
        public void BuildModel(params string[] inputColumns)
        {
            // Randomly split the dataset by a val. One for training and the other to test the trained model against
            var trainingData = _mlContext.Clustering.TrainTestSplit(_dataView, 0.2);
            var pipeline = _mlContext.Transforms.Concatenate("Features", inputColumns)
               .Append(_mlContext.Clustering.Trainers.KMeans(featureColumnName: "Features"));
            _predictions = pipeline.Fit(trainingData.TrainSet).CreatePredictionEngine<TRowModel, ClusterPrediction>(_mlContext);  
        }

        public ClusterPrediction Evaluate(TRowModel model)
        {
             return _predictions.Predict(model);
        }
    }
}
