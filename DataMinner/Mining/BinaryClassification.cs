using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;

namespace DataMinner.Mining
{
    public class BinaryClassification<TRowModel> where TRowModel : class
    {
        private readonly MLContext _mlContext;
        private IDataView _dataView;
        private PredictionEngine<BinaryClassificationData, BinaryClassificationPrediction> _predEngine;

        public BinaryClassification(IEnumerable<TRowModel> rows)
        {
            _mlContext = new MLContext();
            _dataView = _mlContext.Data.LoadFromEnumerable(rows);
        }

        // Prepare a pipeline for training, train it, and create a prediction object
        public void BuildModel()
        {
            // Randomly split the dataset by a val. One for training and the other to test the trained model against
            TrainCatalogBase.TrainTestData splitDataView = _mlContext.BinaryClassification.TrainTestSplit(_dataView, testFraction: 0.2);
            var dataProcessPipeline = _mlContext.Transforms.Text.FeaturizeText(outputColumnName: DefaultColumnNames.Features, inputColumnName: nameof(BinaryClassificationData.SentimentText));
            var trainer = _mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "Sentiment", featureColumnName: DefaultColumnNames.Features);
            var pipeline = dataProcessPipeline.Append(trainer);
            ITransformer trainedModel = pipeline.Fit(splitDataView.TrainSet);
            _predEngine = trainedModel.CreatePredictionEngine<BinaryClassificationData, BinaryClassificationPrediction>(_mlContext);
        }

        public BinaryClassificationPrediction Evaluate(BinaryClassificationData data)
        {
            return _predEngine.Predict(data);
        }
    }
}
