using Database.BaseDb;
using DataMinner.Mining.Enums;
using FUSQL.Mining;
using FUSQL.Models;
using FUSQL.SQLTranslate.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.SQLTranslate.Translator
{
    public class Translation<TRowModel> where TRowModel : class, new()
    {
        public Operation Operation { get; private set; }    
        public Translation(Operation operation)
        {
            Operation = operation;
        }

        public void RunSQL(IDb db, Action<TRowModel> onRowRead)
        {
            db.Command<TRowModel>(Operation.SQLCommand, (model) =>
            {
                onRowRead.Invoke(model);
            });
        }
        
        
    }
}
