using DataMinner.Mining.BinaryClassification;
using DataMinner.Mining.MultiClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.InternalModels
{
    public class FusqlInternal<TRowModel> where TRowModel : class, new()
    {
        private Dictionary<string, MultiClassification<TRowModel>> _multiClassifiers;
        private Dictionary<string, BinaryClassification<TRowModel>> _binaryClassifiers;
        private static FusqlInternal<TRowModel> instance = null;
        private static readonly object padlock = new object();

        private FusqlInternal()
        {
            _multiClassifiers = new Dictionary<string, MultiClassification<TRowModel>>();
            _binaryClassifiers = new Dictionary<string, BinaryClassification<TRowModel>>();
        }
        public static FusqlInternal<TRowModel> GetInstance()
        {

            lock(padlock)
            {
                if (instance == null)
                {
                    instance = new FusqlInternal<TRowModel>();
                }
                return instance;
            }
            
        }
        public void AddMultiClassifier(string classifierName, MultiClassification<TRowModel> classifier)
        {
            if (!_multiClassifiers.ContainsKey(classifierName))
            {
                _multiClassifiers.Add(classifierName, classifier);
            }
        }
        public MultiClassification<TRowModel> GetMultiClassifer(string classifierName)
        {
            if (_multiClassifiers.ContainsKey(classifierName))
            {
                return _multiClassifiers[classifierName];
            }
            return null;
        }
        public void DeleteMultiClassifier(string name)
        {
            _multiClassifiers.Remove(name);
        }
        public void AddBinaryClassifier(string classifierName, BinaryClassification<TRowModel> classifier)
        {
            if (!_binaryClassifiers.ContainsKey(classifierName))
            {
                _binaryClassifiers.Add(classifierName, classifier);
            }
        }
        public BinaryClassification<TRowModel> GetBinaryClassification(string classifierName)
        {
            if (_binaryClassifiers.ContainsKey(classifierName))
            {
                return _binaryClassifiers[classifierName];
            }
            return null;
        }
        public void DeleteBinaryClassifier(string name)
        {
            _binaryClassifiers.Remove(name);
        }

    }
}
