using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Results
{
    public class RunClassifierEntriesResultView<TRowModel> : ResultView<TRowModel> where TRowModel : class, new()
    {
        public Dictionary<string, List<TRowModel>> Predictions { get; set; } = new Dictionary<string, List<TRowModel>>();
        public RunClassifierEntriesResultView() : base(DataMinner.Mining.Enums.MiningOp.ClassifyEntries) { }
        public override string ToString()
        {
            string result = "";
            foreach (string key in Predictions.Keys)
            {
                result += key + "\n";
                foreach (TRowModel model in Predictions[key])
                {
                    result +=  model.ToString() + "\n";
                }
            }
            return result;
        }
    }
}
