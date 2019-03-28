using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataMinner.Mining
{
    public class BinaryClassification
    {
        private readonly MLContext _mlContext;
        private IDataView _dataView;

        public BinaryClassification()
        {
            _mlContext = new MLContext();
            //_dataView = _mlContext.Data.LoadFromEnumerable<TRowModel>(rows);
        }

        // Prepare a pipeline for training, train it, and create a prediction object
        public void BuildModel()
        {
            // Randomly split the dataset by a val. One for training and the other to test the trained model against
            var dataPath = Path.GetFullPath("./amazon_cells_labelled.txt");
            IDataView dataView = _mlContext.Data.LoadFromTextFile<BinaryClassificationData>(dataPath, hasHeader: true);
            TrainCatalogBase.TrainTestData splitDataView = _mlContext.BinaryClassification.TrainTestSplit(dataView, testFraction: 0.2);
            var dataProcessPipeline = _mlContext.Transforms.Text.FeaturizeText(outputColumnName: DefaultColumnNames.Features, inputColumnName: nameof(BinaryClassificationData.SentimentText));
            var trainer = _mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: DefaultColumnNames.Label, featureColumnName: DefaultColumnNames.Features);
            var pipeline = dataProcessPipeline.Append(trainer);
            ITransformer trainedModel = pipeline.Fit(splitDataView.TrainSet);
            IDataView predictions = trainedModel.Transform(splitDataView.TestSet);
            CalibratedBinaryClassificationMetrics metrics = _mlContext.BinaryClassification.Evaluate(data: predictions, label: DefaultColumnNames.Label, score: DefaultColumnNames.Score);

            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.Auc:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            Console.WriteLine("=============== End of model evaluation ===============");

            var predEngine = trainedModel.CreatePredictionEngine<BinaryClassificationData, BinaryClassificationPrediction>(_mlContext);
            //BinaryClassificationData sentiment = new BinaryClassificationData { SentimentText = "not bad!" };
            IEnumerable<BinaryClassificationData> sentiments = new[]
            {
                new BinaryClassificationData
                {
                    SentimentText = "This was a horrible meal"
                },
                new BinaryClassificationData
                {
                    SentimentText = "I love this spaghetti"
                },
                new BinaryClassificationData
                {
                    SentimentText = "This isn't even relevant at all dude"
                },
                new BinaryClassificationData
                {
                    SentimentText = "Pretty sweet cool Dog shit machine learning"
                }
            };

            // Predict the result of the classification of each object
            foreach (var sentiment in sentiments)
            {
                var resultPrediction = predEngine.Predict(sentiment);
                Console.WriteLine($"Sentiment: {sentiment.SentimentText} | Prediction: {(Convert.ToBoolean(resultPrediction.Prediction) ? "Positive" : "Negative")} | Probability: {resultPrediction.Probability} ");
            }
            Console.ReadLine();
        }

        //public ClusterPrediction Evaluate(TRowModel model)
        //{
            //return _predictions.Predict(model);
        //}
    }
}
