using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMinner.Mining.BinaryClassification
{
    public class BinaryClassification<TRowModel> where TRowModel: class
    {
        private readonly MLContext _mLContext;
        private readonly IDataView _dataView;
        private ITransformer _trainedModel;
        public BinaryClassification(IEnumerable<TRowModel> rows)
        {
            _mLContext = new MLContext();
            _dataView = _mLContext.Data.LoadFromEnumerable(rows);
        }
        public void CreateBuild(string goalColumn, params string[] inputColumns)
        {
            var splitDataView = _mLContext.BinaryClassification.TrainTestSplit(_dataView, testFraction: 0.2);
            var originalTextpipeline = _mLContext.Transforms.Text.FeaturizeText(DefaultColumnNames.Features, inputColumnName: inputColumns[0]);
            EstimatorChain<ITransformer> estimatorChainTextPipeline = null;
            for (int i = 1; i < inputColumns.Length; i++)
            {
                if (estimatorChainTextPipeline == null)
                {
                    estimatorChainTextPipeline = originalTextpipeline.Append(_mLContext.Transforms.Text.FeaturizeText(DefaultColumnNames.Features, inputColumnName: inputColumns[i]));
                }
                else
                {
                    estimatorChainTextPipeline = estimatorChainTextPipeline.Append(_mLContext.Transforms.Text.FeaturizeText(DefaultColumnNames.Features, inputColumnName: inputColumns[i]));
                }
            }
            var trainer = _mLContext.BinaryClassification.Trainers.FastTree(labelColumnName: goalColumn, featureColumnName: DefaultColumnNames.Features);
            if (estimatorChainTextPipeline != null)
            {
                var finalPipeline = estimatorChainTextPipeline.Append(trainer);
                _trainedModel = finalPipeline.Fit(splitDataView.TrainSet);
            }
            else
            {
                var finalPipeline = originalTextpipeline.Append(trainer);
                _trainedModel = finalPipeline.Fit(splitDataView.TrainSet);
            }
        }
        public BinaryClassificationPrediction Evaluate(TRowModel data)
        {
            var predEngine = _trainedModel.CreatePredictionEngine<TRowModel, BinaryClassificationPrediction>(_mLContext);
            return predEngine.Predict(data);
        }
    }
}
