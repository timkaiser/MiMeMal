using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class sc_bluethoot_receiver : MonoBehaviour
{
    public sc_texture_loader texture_loader;
    public enum SignalFlag
    {
        COMMAND, FILENAME, TEXTURE
    };

    public String portName = "COM3";

    private SerialPort port = null;
    private int baudRate = 9600;
    private int readTimeout = 300;
    private int resolution = 2048;
    private int readSize = 32;

    private bool reading = false;
    List<byte> bytes = new List<byte>();

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
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Update()
    {
        try
        {
            byte[] buffer = new byte[readSize];
            if (port.Read(buffer, 0, readSize) > 0)
            {
                if (reading)
                {
                    bytes.AddRange(buffer);
                    return;
                }
                int signalFlag = (int)char.GetNumericValue(Convert.ToChar(buffer[0]));
                if (signalFlag == (int)SignalFlag.COMMAND)
                {
                    string resourcePath = byteToString(buffer);
                    Debug.Log("Command " + resourcePath);

                    texture_loader.setTexture(texture_loader.loadResource(resourcePath));
                }
                else if (signalFlag == (int)SignalFlag.FILENAME)
                {
                    string filename = byteToString(buffer);
                    Debug.Log("Filename " + filename);

                    texture_loader.setTexture(texture_loader.loadTexture(
                        Application.persistentDataPath + filename, resolution));
                }
                else if (signalFlag == (int)SignalFlag.TEXTURE)
                {
                    reading = true;
                    String fileName = byteToString(buffer);
                    Debug.Log("Texture " + fileName);
                }
            } else if (reading)
            {
                Debug.Log("Done reading " + bytes.ToArray().Length);
                reading = false;
            }
        }
        catch (TimeoutException)
        {
            //do nothing
            if (reading)
            {
                Debug.Log("Error reading");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void OnDisable()
    {
        //Close Port
        if (port != null && port.IsOpen)
        {
            port.Close();
        }
    }

    private string byteToString(byte[] message)
    {
        String result = "";
        for (int i = 1; i < message.Length; i++)
        {
            result += Convert.ToChar(message[i]);
        }
        return result;
    }
}
