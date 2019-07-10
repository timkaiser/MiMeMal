using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;

public class sc_bluetooth : MonoBehaviour
{
    public String portName = "COM3";
    public int baudRate = 115200;

    private SerialPort port = null;
    private int readTimeOut = 100;

    void Start()
    {
        String[] ports = SerialPort.GetPortNames();
        for (int i = 0; i < ports.Length; i++)
        {
            Debug.Log(ports[i]);
        }

        try
        {
            port = new SerialPort();
            port.PortName = portName;
            port.BaudRate = baudRate;
            port.ReadTimeout = readTimeOut;
            port.Open();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OnDisable()
    {
        if (port != null && port.IsOpen)
        {
            port.Close();
        }
    }
}