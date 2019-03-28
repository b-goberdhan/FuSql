using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataMinner.Mining
{
    public class MulticlassClassification
    {
        private readonly MLContext _mlContext;
        private IDataView _dataView;

        public MulticlassClassification()
        {
            _mlContext = new MLContext();
            //_dataView = _mlContext.Data.LoadFromEnumerable<TRowModel>(rows);
        }

        // Prepare a pipeline for training, train it, and create a prediction object
        public void BuildModel()
        {
            // Template code taken from: https://github.com/dotnet/samples/blob/master/machine-learning/tutorials/GitHubIssueClassification/Program.cs

            // Randomly split the dataset by a val. One for training and the other to test the trained model against
            var trainDataPath = Path.GetFullPath("./issues_train.tsv");
            var testDataPath = Path.GetFullPath("./issues_test.tsv");
            IDataView trainDataView = _mlContext.Data.LoadFromTextFile<MulticlassClassificationData>(trainDataPath, hasHeader: true);
            IDataView testDataView = _mlContext.Data.LoadFromTextFile<MulticlassClassificationData>(testDataPath, hasHeader: true);

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Area", outputColumnName: "Label")
            .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title", outputColumnName: "TitleFeaturized"))
            .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Description", outputColumnName: "DescriptionFeaturized"))
            .Append(_mlContext.Transforms.Concatenate("Features", "TitleFeaturized", "DescriptionFeaturized"))
            .AppendCacheCheckpoint(_mlContext);

            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(DefaultColumnNames.Label, DefaultColumnNames.Features))
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var _trainedModel = trainingPipeline.Fit(trainDataView);

            var _predEngine = _trainedModel.CreatePredictionEngine<MulticlassClassificationData, MulticlassClassificationPrediction>(_mlContext);

            MulticlassClassificationData issue = new MulticlassClassificationData()
            {
                Title = "WebSockets communication is slow in my machine",
                Description = "The WebSockets communication used under the covers by SignalR looks like is going slow in my development machine.."
            };
  
            var prediction = _predEngine.Predict(issue);
            Console.WriteLine($"=============== Single Prediction just-trained-model - Result: {prediction.Area} ===============");

            Console.ReadLine();
        }

        //public ClusterPrediction Evaluate(TRowModel model)
        //{
        //return _predictions.Predict(model);
        //}
    }
}
