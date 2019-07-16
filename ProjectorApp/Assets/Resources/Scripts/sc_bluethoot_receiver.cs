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

    public string portName = "COM3";

    private SerialPort port = null;
    private int baudRate = 9600;
    private int readTimeout = 300;
    private const int resolution = 2048;
    private int readSize = 32;

    private bool isReadingTexture = false;
    private int readingCounter = 0;
    private const int MAX_READ = 15;
    List<byte> textureBytes;
    private string textureFileName = "";
    private int textureSize;

    // Start is called before the first frame update
    void Start()
    {
        textureBytes = new List<byte>();
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
            byte[] receivedData = new byte[readSize];
            int numBytes = port.Read(receivedData, 0, readSize);
            if (numBytes > 0)
            {
                byte[] data = new byte[numBytes];
                Array.Copy(receivedData, data, numBytes);
                if (isReadingTexture)
                {
                    readingCounter = 0;
                    textureBytes.AddRange(data);
                    return;
                }
                int signalFlag = (int)char.GetNumericValue(Convert.ToChar(data[0]));
                if (signalFlag == (int)SignalFlag.COMMAND)
                {
                    string resourcePath = byteToString(data);
                    Debug.Log("Command " + resourcePath);

                    texture_loader.setTexture(texture_loader.loadResource(resourcePath));
                }
                else if (signalFlag == (int)SignalFlag.FILENAME)
                {
                    string filename = byteToString(data);
                    Debug.Log("Filename " + filename);

                    texture_loader.setTexture(texture_loader.loadTexture(
                        Application.persistentDataPath + "/" + filename, resolution));
                }
                else if (signalFlag == (int)SignalFlag.TEXTURE)
                {
                    isReadingTexture = true;
                    readingCounter = 0;
                    string message = byteToString(data);
                    string[] parts = message.Split(' ');
                    textureFileName = parts[0];
                    textureSize = Convert.ToInt32(parts[1]);
                    Debug.Log("Texture " + textureFileName);
                    port.ReadTimeout = 10000;
                    readSize = 2000000;
                }
            }
        }
        catch (TimeoutException)
        {
            if (isReadingTexture&&readingCounter>=MAX_READ)
            {
                Debug.Log("Done reading");
                isReadingTexture = false;
                readingCounter = 0;
                port.ReadTimeout = 100;
                readSize = 32;
                Debug.Log("size is: " + textureBytes.Count + ", expected: " + textureSize);
                if (textureBytes.Count > 0)
                {
                    string path = Application.persistentDataPath + "/" + textureFileName;
                    texture_loader.setTexture(texture_loader.loadFromBytes(textureBytes.ToArray(), resolution));
                    File.WriteAllBytes(path, textureBytes.ToArray());
                }
            } else if(isReadingTexture&&readingCounter<MAX_READ)
            {
                if (textureBytes.Count > 0)
                {
                    readingCounter = MAX_READ;
                } else
                {
                    readingCounter++;
                }
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
