using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class IMUReader : MonoBehaviour
{
    SerialPort serial;
    public string portName = "COM11"; // ganti sesuai port Arduino kamu
    public int baudRate = 115200;

    public float pitch, roll, yaw;

    void Start()
    {
        serial = new SerialPort(portName, baudRate);
        serial.ReadTimeout = 50;
        serial.Open();
    }
    void Update()
    {
        if (serial.IsOpen)
        {
            try
            {
                string data = serial.ReadLine(); // Contoh: "P:1,23 R:-3,45 Y:0,00"
                ParseData(data);
            }
            catch (System.Exception) { }
        }
    }
    void ParseData(string raw)
    {
        string[] parts = raw.Split(' ');
        foreach (var part in parts)
        {
            if (part.StartsWith("P:")) pitch = float.Parse(part.Substring(2));
            if (part.StartsWith("R:")) roll = float.Parse(part.Substring(2));
            if (part.StartsWith("Y:")) yaw = float.Parse(part.Substring(2));
        }
    }

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
            serial.Close();
    }
}
