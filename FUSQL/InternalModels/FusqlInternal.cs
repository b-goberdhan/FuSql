using DataMinner.Mining.MultiClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.InternalModels
{
    public sealed class FusqlInternal
    {
        

        private static FusqlInternal instance = null;
        private static readonly object padlock = new object();

        private FusqlInternal()
        {

        }
        public static FusqlInternal Instance
        {
            get
            {
                lock(padlock)
                {
                    if (instance == null)
                    {
                        instance = new FusqlInternal();
                    }
                    return instance;
                }
            }
        }

    }
}
