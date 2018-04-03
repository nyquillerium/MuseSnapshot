using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System;
using UnityEngine;
using UnityEngine.UI;

public class serialRead : MonoBehaviour {
    public int bpm = 0;
    // Use this for initialization
    public int portnum = 6;
    SerialPort port1;

    public List<int> bpmList;

    public int restbpm = 0;

    bool calibrating;

    void Start () {
        Debug.Log(portnum);
        string portstring;
        if (portnum >= 10)
            portstring = "\\\\.\\COM" + portnum;
        else
            portstring = "COM" + portnum;
        port1 = new SerialPort(portstring, 9600);
        port1.ReadTimeout = 50;
        port1.Open();
        StartCoroutine
        (
            
            AsynchronousReadFromArduino
            (   (string s) => ReadInput(s),     // Callback
                () => Debug.LogError("Error!"), // Error callback
                10000f                          // Timeout (milliseconds)
            )
        );
    }

    void ReadInput(string s)
    {
        if (s == "Ack")
        {
            
        }
        else
        {
            GameObject.Find("Calibrate").GetComponent<Button>().interactable = true;
            Debug.Log(s);
            bpm = int.Parse(s);
            GameObject.Find("HRText").GetComponent<Text>().text = "BPM: " + bpm;

            if (bpm >= restbpm * 1.1)
            {
                GameObject.Find("HRText").GetComponent<Text>().color = Color.red;
            }
            else
            {
                GameObject.Find("HRText").GetComponent<Text>().color = Color.black;
            }

            if (calibrating && bpmList.Count < 10)
            {
                bpmList.Add(int.Parse(s));
                Debug.Log("Added: " + int.Parse(s));
            }
            else if (calibrating)
            {
                Debug.Log("anime");
                calibrating = false;
                int bpmsum = 0;
                foreach (int i in bpmList)
                {
                    bpmsum += i;
                }
                restbpm = bpmsum / bpmList.Count;
                Debug.Log(restbpm);
            }
        }
        
    }

    public void Calibrate()
    {
        calibrating = true;
    }

    public string ReadFromArduino(int timeout = 0)
    {
        port1.ReadTimeout = timeout;
        try
        {
            return port1.ReadLine();
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            try
            {
                dataString = port1.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield return null;
            }
            else
                yield return new WaitForSeconds(0.05f);

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }
}
