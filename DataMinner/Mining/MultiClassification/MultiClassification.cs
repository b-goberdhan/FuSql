using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMinner.Mining.MultiClassification
{
    public class MultiClassification<TRowModel> where TRowModel: class
    {
        private readonly MLContext _mlContext;
        private readonly IDataView _dataView;
        private TransformerChain<KeyToValueMappingTransformer> _transformers;

        public MultiClassification(IEnumerable<TRowModel> rows)
        {
            _mlContext = new MLContext();
            _dataView = _mlContext.Data.LoadFromEnumerable(rows);
        }
        public void CreateBuild(string goalColumn, params string[] inputColumns)
        {
            var splitDataView = _mlContext.MulticlassClassification.TrainTestSplit(_dataView, testFraction: 0.2);

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: goalColumn, outputColumnName: "Label");
            List<string> features = new List<string>();
            EstimatorChain<ITransformer> inputColumnEstimatorChain = null;
            foreach (var input in inputColumns)
            {
                string feature = input + "Featurized";
                features.Add(feature);
                if (inputColumnEstimatorChain == null)
                {
                    inputColumnEstimatorChain = pipeline.Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: input, outputColumnName: feature));
                }
                else
                {
                    inputColumnEstimatorChain = inputColumnEstimatorChain.Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: input, outputColumnName: feature));
                }
                   // .Append(_mlContext.Transforms.Concatenate("Features", feature));
            }
            var concatonatedFeaturesChain = inputColumnEstimatorChain.Append(_mlContext.Transforms.Concatenate("Features", features.ToArray()));

            var trainingPipeline = concatonatedFeaturesChain.Append(_mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(DefaultColumnNames.Label, DefaultColumnNames.Features))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            _transformers = trainingPipeline.Fit(splitDataView.TrainSet);
        }

        public MultiClassPrediction Evaluate(TRowModel data)
        {
            return _transformers.CreatePredictionEngine<TRowModel, MultiClassPrediction>(_mlContext).Predict(data);
        }
    }
}
