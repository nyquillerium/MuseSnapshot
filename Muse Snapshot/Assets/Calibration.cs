using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : MonoBehaviour {
    private OSCConnection manager;
    public float offset = 0;
    public float smileset = 0;
    public float resultset = 0;
    private bool issmile;
    public bool isSmiling = false;
    private bool playing = false;
    public int score = 0;
    
    void Start()
    {
        manager = GameObject.FindObjectOfType<OSCConnection>();

    }

    void Update()
    {
        if (manager.Calibrating)
        {
            if (manager.CalibrationEEG.Count >= 1000 && !playing)
            {
                manager.Calibrating = false;

                foreach (float f in manager.CalibrationEEG)
                {
                    if (!issmile)
                        offset += f;
                    else
                        smileset += f;
                }
                if (!issmile)
                {
                    offset /= manager.CalibrationEEG.Count;
                    GameObject.Find("OffsetText").GetComponent<Text>().text = "Offset: " + offset;
                }
                else
                {
                    smileset /= manager.CalibrationEEG.Count;
                    GameObject.Find("SmilesetText").GetComponent<Text>().text = "Smileset: " + smileset;
                }

                manager.CalibrationEEG.Clear();
            }
            else if (playing && !manager.Calibrating)
            {
                foreach (float f in manager.CalibrationEEG)
                {
                    resultset += f;
                }
                resultset /= manager.CalibrationEEG.Count;
                if (Smiling(smileset, offset, resultset))
                    score++;
                GameObject.Find("ScoreText").GetComponent<Text>().text = "Score: " + score.ToString();
            }
        }

        if (offset != 0 && !manager.Calibrating)
        {
            GameObject.Find("SCalibrationButton").GetComponent<Button>().interactable = true;
        }

        if (offset != 0 && smileset != 0)
        {
            if (Smiling(smileset, offset, manager.m100))
                isSmiling = true;
            else
                isSmiling = false;
        }
    }
    public void Calibrate()
    {
        issmile = false;
        manager.Calibrating = true;
    }

    public void CalibrateSmile()
    {
        issmile = true;
        manager.Calibrating = true;
    }

    public void playGame()
    {
        playing = true;
        manager.Calibrating = true;
    }

    public bool Smiling(float smilef, float normalf, float compareValue)
    { 
        float calcA = Math.Abs((long)smilef - compareValue);
        float calcB = Math.Abs((long)normalf - compareValue);

        if (calcA < calcB)
        {
            return true;
        }

        return false;
    }
}
