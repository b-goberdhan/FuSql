using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL_Tester
{
    public class GroceryStock
    {
        [LoadColumn(0)]
        string MemberNumber { get; set; }
        [LoadColumn(1)]
        string Date { get; set; }
        [LoadColumn(2)]
        string Items { get; set; }
    }
}
