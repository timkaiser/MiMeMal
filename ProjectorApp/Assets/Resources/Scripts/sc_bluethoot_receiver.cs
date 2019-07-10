using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;

public class sc_bluethoot_receiver : MonoBehaviour
{
    public String portName = "COM3";
    public int baudRate = 115200;

    private SerialPort port = null;
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
        while(running)
        {
            try
            {
                Debug.Log(port.ReadLine());
            } catch (TimeoutException)
            {
                //do nothing
            }
            Thread.Sleep(500);
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
