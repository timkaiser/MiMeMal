using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

public class sc_bluethoot_receiver : MonoBehaviour
{
    public enum SignalFlag
    {
        COMMAND, FILENAME, TEXTURE
    };

    public String portName = "COM3";

    private SerialPort port = null;
    private int baudRate = 9600;
    private int readTimeout = 100;
    private Boolean running = true;

    // Start is called before the first frame update
    void Start()
    {
        String[] ports = SerialPort.GetPortNames();
        for (int i = 0; i < ports.Length; i++)
        {
            Debug.Log(ports[i]);
        }

        //Open Port
        Debug.Log("Opening port");
        port = new SerialPort();
        port.PortName = portName;
        port.BaudRate = baudRate;
        port.ReadTimeout = readTimeout;
        try
        {
            port.Open();
        } catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        //Start thread for reading data
        running = true;
        Thread t = new Thread(new ThreadStart(ReceiveData));
        t.Start();
    }

    void ReceiveData()
    {
        Debug.Log("Start reading");
        int readSize = 32;
        while(running)
        {
            try
            {
                byte[] buffer = new byte[readSize];
                if (port.Read(buffer, 0, readSize) > 0)
                {
                    int signalFlag = (int)char.GetNumericValue(Convert.ToChar(buffer[0]));
                    if (signalFlag == (int)SignalFlag.COMMAND)
                    {
                        Debug.Log("Command");
                    }
                    else if (signalFlag == (int)SignalFlag.FILENAME)
                    {
                        Debug.Log("Filename");
                    }
                    else if (signalFlag == (int)SignalFlag.TEXTURE)
                    {
                        Debug.Log("Texture");
                        String fileName = "";
                        for (int i = 1; i < readSize; i++)
                        {
                            fileName += Convert.ToChar(buffer[i]);
                        }
                        Debug.Log(fileName);
                        Thread.Sleep(3000);
                        Debug.Log("Reading texture");
                        buffer = new byte[readSize];
                        List<byte> bytes = new List<byte>();
                        while (port.Read(buffer, 0, readSize) > 0)
                        {
                            bytes.AddRange(buffer);
                        }
                        Debug.Log(bytes.ToArray().Length);
                        continue;
                    }
                    String message = "";
                    for (int i = 1; i < readSize; i++)
                    {
                        message += Convert.ToChar(buffer[i]);
                    }
                    Debug.Log(message);
                }
            } catch (TimeoutException)
            {
                //do nothing
            }
        }
        Debug.Log("End reading");
    }

    private void OnDisable()
    {
        //Close Port
        running = false;
        if (port != null && port.IsOpen)
        {
            port.Close();
        }
    }
}
