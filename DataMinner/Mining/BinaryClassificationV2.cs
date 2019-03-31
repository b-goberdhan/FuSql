using Microsoft.Data.DataView;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMinner.Mining
{
    public class BinaryClassificationV2<TRowModel> where TRowModel : class
    {
        private readonly MLContext _mlContext;
        private IDataView _dataView;

        public BinaryClassificationV2(IEnumerable<TRowModel> rows)
        {
            _mlContext = new MLContext();
            _dataView = _mlContext.Data.LoadFromEnumerable<TRowModel>(rows);
        }
        
        public void BuildModel()
        {
            _mlContext.BinaryClassification.TrainTestSplit(_dataView, testFraction: 0.2);

        }
    }
}
