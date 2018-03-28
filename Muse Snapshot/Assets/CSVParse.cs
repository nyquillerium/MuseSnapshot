using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public struct Entry
{
    public List<float> Values;
}

public class DataTable
{
    public List<Entry> Entries;
    public List<string> FeatureList;

    public void print()
    {
        {
            float win = Screen.width * 0.6f;
            float w1 = win * 0.05f; float w2 = win * 0.15f; float w3 = win * 0.35f;
            foreach (Entry e in Entries)
            {
                if (e.Values.Count >= 3)
                {
                    GUILayout.BeginHorizontal();
                    foreach (float f in e.Values)
                    {
                        GUILayout.Label(f.ToString(), GUILayout.Width(w1));
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}

public class CSVParse : MonoBehaviour
{
    public DataTable NewData = new DataTable();
    public string FileName = "muse.data";
    public string FeaturesName = "features.txt";
    private bool Load(string fileName, string featuresFile)
    {
        try
        {
            string line;
            StreamReader theReader = new StreamReader(Application.dataPath + "\\" +  fileName, Encoding.Default);
            StreamReader featureReader = new StreamReader(Application.dataPath + "\\" + featuresFile, Encoding.Default);
            using (theReader)
            {
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        string[] entries = line.Split(',');
                        if (entries.Length > 0)
                        {
                            Entry newEntry = new Entry();
                            newEntry.Values = new List<float>();
                            foreach (string s in entries)
                            {
                                newEntry.Values.Add(float.Parse(s));
                            }
                            NewData.Entries.Add(newEntry);
                        }   
                    }
                }
                while (line != null); 
                theReader.Close();

                do
                {
                    line = featureReader.ReadLine();

                    if (line != null)
                    {
                        string[] entries = line.Split(',');
                        if (entries.Length > 0)
                        {
                            foreach (string s in entries)
                            {
                                NewData.FeatureList.Add(s);
                            }
                        }
                    }
                }
                while (line != null);
                theReader.Close();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    void Awake ()
    {
        
    }

    void OnGUI ()
    {
        NewData.Entries = new List<Entry>();
        Load(FileName, FeaturesName);
        NewData.print();
    }
}
