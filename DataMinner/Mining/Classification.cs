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
    public class Classification<TResult, TData> : IMinningOperation<TResult, TData> where TData : class
    {
        private readonly MLContext _mlContext;
        private IDataView _trainData;
        private readonly int _clusterCount;
        private EstimatorChain<ClusteringPredictionTransformer<KMeansModelParameters>> _pipeline;
        public Classification(int clusterCount)
        {
            _mlContext = new MLContext();
            _clusterCount = clusterCount;
        }
        public TResult Run()
        {
            return default(TResult);
        }
        public TResult LoadData(IEnumerable<TData> data)
        {
            _mlContext.Data.LoadFromEnumerable(data);
            return default(TResult);
        }
        public void Train(params string[] inputs)
        {
            _pipeline = _mlContext.Transforms.Concatenate("Features", inputs)
                 .Append(_mlContext.Clustering.Trainers.KMeans(featureColumnName: "Features", clustersCount: _clusterCount));
        }
    }
}
