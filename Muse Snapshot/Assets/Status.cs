using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    private OSCConnection manager;

	// Use this for initialization
	void Start () {
        manager = GameObject.FindObjectOfType<OSCConnection>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (manager.SensorA == 1)
	    {
	        GameObject.Find("ForeheadText").GetComponent<Text>().color = Color.green;
	        GameObject.Find("EEGText").GetComponent<Text>().text = manager.m100.ToString();
	        GameObject.Find("CalibrationButton").GetComponent<Button>().interactable = true;
	    }
	    else
	    {
	        GameObject.Find("ForeheadText").GetComponent<Text>().color = new Color(177, 0, 0);
	        GameObject.Find("EEGText").GetComponent<Text>().text = "";
	        GameObject.Find("CalibrationButton").GetComponent<Button>().interactable = false;
	    }

	}
}
