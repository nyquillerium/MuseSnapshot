using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class OSCConnection : MonoBehaviour
{
    public string RemoteIP = "127.0.0.1"; 
    public int ListenerPort = 5000; 
    Osc handler;
    UDPPacketIO udp;
    public bool signal = false;
    public float SensorA, s1, c1;
    public List<float> CalibrationEEG;
    public List<float> m100EEG;
    public bool Calibrating = false;
    public float m100;
    public bool playing = false;

    void Start()
    { 
        udp = new UDPPacketIO();
        udp.init(RemoteIP, ListenerPort);
        handler = new Osc();
        handler.init(udp);
        handler.SetAllMessageHandler(AllMessageHandler);
    }

    void Update()
    {

    }
    void OnDisable()
    {
        udp.Close();
    }
    public void AllMessageHandler(OscMessage oscMessage) //All values passed
    {
        signal = true;
        string msgString = Osc.OscMessageToString(oscMessage); 
        string msgAddress = oscMessage.Address; 
        if (msgAddress == "/muse/elements/touching_forehead")
            SensorA = (int)oscMessage.Values[0]; // 0 = not touching
        if (msgAddress == "/muse/eeg")
        {
            s1 = (float)oscMessage.Values[0];
            if (Calibrating)
            {
                CalibrationEEG.Add((float)oscMessage.Values[0]);
            }
            if (m100EEG.Count >= 100)
            {
                foreach (float f in m100EEG)
                {
                    m100 += f;
                }
                m100 /= m100EEG.Count;
                m100EEG.Clear();
            }
            m100EEG.Add((float)oscMessage.Values[0]);
            
        }
        if (msgAddress == "/muse/elements/alpha_relative")
        {
            c1 = (float)oscMessage.Values[0];
        }
    }
}