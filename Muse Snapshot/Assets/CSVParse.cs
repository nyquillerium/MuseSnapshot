using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using SharpLearning.Containers;
using SharpLearning.Containers.Matrices;
using System.Linq;
using SharpLearning.DecisionTrees.Learners;
using SharpLearning.RandomForest.Learners;
using SharpLearning.RandomForest.Models;
using SharpLearning.CrossValidation.TrainingTestSplitters;
using SharpLearning.Metrics.Classification;
using UnityEngine.UI;

public struct Entry
{
    public List<float> Values;
    
}

public class DataTable
{
    public List<double> Entries;
    public List<double> classifiers;
}

public class CSVParse : MonoBehaviour
{
    public DataTable NewData = new DataTable();
    public string FileName = "muse.data";
    public string FeaturesName = "features.txt";
    const int NUM_ENTRIES = 286;
    public ClassificationForestModel model;
    private bool Load(string fileName, string featuresFile)
    {
        try
        {
            string line;
            StreamReader theReader = new StreamReader(Application.dataPath + "/StreamingAssets/" +  fileName, Encoding.Default);
            GameObject.Find("PathText").GetComponent<Text>().text = "Path: " + Application.dataPath + "/StreamingAssets/" + fileName;
            Debug.Log("Path: " + Application.dataPath + "/StreamingAssets/" + fileName);
            using (theReader)
            {
                int linesRead = 0;
                double currentTag = 0;
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        if (linesRead == 0)
                        {
                            string[] entries = line.Split(',');
                            currentTag = double.Parse(entries[0]);
                        }
                        else
                        {
                            string[] entries = line.Split(',');
                            if (entries.Length > 0)
                            {
                                foreach (string s in entries)
                                {
                                    NewData.Entries.Add(double.Parse(s));
                                }
                                NewData.classifiers.Add(currentTag);
                            }

                            if (linesRead >= NUM_ENTRIES)
                                linesRead = -1;
                        }
                    }
                    linesRead++;
                }
                while (line != null); 
                theReader.Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    void Awake ()
    {
        NewData.Entries = new List<double>();
        NewData.classifiers = new List<double>();
        Load(FileName, FeaturesName);

        // Create a random forest learner for classification with 100 trees
        ClassificationRandomForestLearner learner = new ClassificationRandomForestLearner(trees: 175, maximumTreeDepth:1900 , featuresPrSplit: 2);
        
        double[] abcd = NewData.Entries.ToArray();
        double[] efgh = NewData.classifiers.ToArray();
        F64Matrix matrix = new F64Matrix(abcd, NewData.classifiers.Count, 4);
        //validation
        var splitter = new StratifiedTrainingTestIndexSplitter<double>(trainingPercentage: 0.8, seed: 24);
        var trainingTestSplit = splitter.SplitSet(matrix, efgh);
        var trainSet = trainingTestSplit.TrainingSet;
        var testSet = trainingTestSplit.TestSet;

        // learn the model
        model = learner.Learn(trainSet.Observations, trainSet.Targets);

        var trainPredictions = model.Predict(trainSet.Observations);
        var testPredictions = model.Predict(testSet.Observations);

        var metric = new TotalErrorClassificationMetric<double>();

        // measure the error on training and test set.
        var trainError = metric.Error(trainSet.Targets, trainPredictions);
        var testError = metric.Error(testSet.Targets, testPredictions);

        Debug.Log("accuracy from a 80/20 training split is: " + (1 - testError));
        GameObject.Find("AccuracyText").GetComponent<Text>().text = "Classification Accuracy of Model: " + (1 - testError);
    }

    void OnGUI ()
    {
    }
}
