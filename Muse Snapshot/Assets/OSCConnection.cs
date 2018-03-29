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
    public float SensorA, s1, s2, s3, s4;
    public List<float> m100EEG;
    public float m100;
    public bool Capturing;

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
        if (msgAddress == "/muse/eeg/TP9")
        {
            s1 = (float)oscMessage.Values[0];
            
        }
        if (msgAddress == "/muse/eeg/Fp1")
        {
            s2 = (float)oscMessage.Values[0];
        }
        if (msgAddress == "/muse/eeg/Fp2")
        {
            s3 = (float)oscMessage.Values[0];
        }
        if (msgAddress == "/muse/eeg/TP10")
        {
            s4 = (float)oscMessage.Values[0];
        }
    }
}