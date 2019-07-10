using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;

public class sc_bluetooth : MonoBehaviour
{
    private SerialPort serialPort = null;
    String portName = "COM3";
    int baudRate = 115200;
    int readTimeOut = 100;
    int bufferSize = 32; // Device sends 32 bytes per packet
    bool programActive = true;
    Thread thread;

    void Start()
    {
        try
        {
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.ReadTimeout = readTimeOut;
            serialPort.Open();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        // Execute a thread to manage the incoming BT data
        thread = new Thread(new ThreadStart(ProcessData));
        thread.Start();
    }


    // Processes the incoming BT data on the virtual serial port.
    void ProcessData()
    {
        Byte[] buffer = new Byte[bufferSize];
        int bytesRead = 0;
        Debug.Log("Thread started");
        while (programActive)
        {
            try
            {
                // Attempt to read data from the BT device
                // - will throw an exception if no data is received within the timeout period
                //bytesRead = serialPort.Read(buffer, 0, bufferSize);
                // Use the appropriate SerialPort read method for your BT device e.g. ReadLine(..) for newline terminated packets
                //if (bytesRead > 0)
                //{
                // Do something with the data in the buffer
                //}
                buffer[0] = 1;
                buffer[1] = 42;
                serialPort.Write("Hello");
                Thread.Sleep(300);
            }
            catch (TimeoutException)
            {
                // Do nothing, the loop will be reset
            }
        }
        Debug.Log("Thread stopped");
    }

    void Update()
    {
    }

    public void OnDisable()
    {
        programActive = false;
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}