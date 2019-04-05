using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataMinner.Mining
{
    public class MulticlassClassification<TRowModel> where TRowModel : class
    {
        private readonly MLContext _mlContext;
        private IDataView _dataView;
        private TransformerChain<KeyToValueMappingTransformer> _transformers;
        private PredictionEngine<MulticlassClassificationData, MulticlassClassificationPrediction> _predEngine;

        public MulticlassClassification(IEnumerable<TRowModel> rows)
        {
            _mlContext = new MLContext();
            _dataView = _mlContext.Data.LoadFromEnumerable<TRowModel>(rows);
        }

        // Prepare a pipeline for training, train it, and create a prediction object
        public void BuildModel()
        {
            // Template code taken from: https://github.com/dotnet/samples/blob/master/machine-learning/tutorials/GitHubIssueClassification/Program.cs
            var splitDataView = _mlContext.MulticlassClassification.TrainTestSplit(_dataView, testFraction: 0.2);

            //var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Area", outputColumnName: "Label")
            //.Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "DescriptionTable", outputColumnName: "DescriptionTableFeaturized"))
            //.Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "GoalTable", outputColumnName: "GoalTableFeaturized"))
            //.Append(_mlContext.Transforms.Concatenate("Features", "DescriptionTableFeaturized", "GoalTableFeaturized"))
            //.AppendCacheCheckpoint(_mlContext);

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "GoalTable", outputColumnName: "Label")
            .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "DescriptionTable", outputColumnName: "DescriptionTableFeaturized"))
            .Append(_mlContext.Transforms.Concatenate("Features", "DescriptionTableFeaturized"));

            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(DefaultColumnNames.Label, DefaultColumnNames.Features))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            _transformers = trainingPipeline.Fit(splitDataView.TrainSet);
            //_predEngine = _trainedModel.CreatePredictionEngine<MulticlassClassificationData, MulticlassClassificationPrediction>(_mlContext);
            //var CUNT = _mlContext.MulticlassClassification.Evaluate(_trainedModel.Transform(_dataView));
        }
        public void BuildModelV2()
        {


        }

        public MulticlassClassificationPrediction Evaluate(TRowModel data)
        {
            var _predEngine = _transformers.CreatePredictionEngine<TRowModel, MulticlassClassificationPrediction>(_mlContext);
            return _predEngine.Predict(data);
        }
    }
}
