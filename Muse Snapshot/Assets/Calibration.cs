using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SharpLearning.RandomForest.Models;

public class Calibration : MonoBehaviour {
    private OSCConnection manager;
    public float offset = 0;
    public float smileset = 0;
    private bool issmile;
    public bool isSmiling = false;
    public Sprite smileTex;
    public Sprite sadTex;
    public Sprite surpriseTex;
    public Sprite angerTex;

    
    int count = 0;
    int currentFace;
    private ClassificationForestModel l_model;
    bool calibrating;

    void Start()
    {
        manager = GameObject.FindObjectOfType<OSCConnection>();
        l_model = GameObject.FindObjectOfType<CSVParse>().model;
    }

    void Update()
    {
        

        if (manager.Capturing)
        {
            double[] currentVal = { manager.s1, manager.s2, manager.s3, manager.s4 };
            if (count <= 25)
                count++;
            else
            {
                currentFace = Convert.ToInt32(l_model.Predict(currentVal));
                count = 0;
            }
            
        }

        switch (currentFace)
        {
            case 1:
                GameObject.Find("FaceState").GetComponent<Image>().sprite = smileTex;
                break;
            case 2:
                GameObject.Find("FaceState").GetComponent<Image>().sprite = sadTex;
                break;
            case 3:
                GameObject.Find("FaceState").GetComponent<Image>().sprite = surpriseTex;
                break;
            case 4:
                GameObject.Find("FaceState").GetComponent<Image>().sprite = angerTex;
                break;
        }

    }
    public void Capture()
    {
        manager.Capturing = true;
        GameObject.Find("FakeButton").GetComponent<Button>().interactable = false;
    }

    

    public void Fake()
    {
        double[] currentVal = { double.Parse(GameObject.Find("Fake1").GetComponent<InputField>().text),
           double.Parse(GameObject.Find("Fake2").GetComponent<InputField>().text),
           double.Parse(GameObject.Find("Fake3").GetComponent<InputField>().text),
           double.Parse(GameObject.Find("Fake4").GetComponent<InputField>().text)
        };
        int current = Convert.ToInt32(l_model.Predict(currentVal));
        currentFace = current;
        Debug.Log("Fake values produced: " + current);
    }

}
