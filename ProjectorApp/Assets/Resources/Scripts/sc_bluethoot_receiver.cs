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
    private const int resolution = 2048;
    private int readSize = 32;

    private bool isReadingTexture = false;
    private int readingCounter = 0;
    private const int MAX_READ = 15;
    List<byte> textureBytes;
    private string textureFileName = "";

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
                if (isReadingTexture)
                {
                    Array.Copy(receivedData, receivedData, numBytes);
                    textureBytes.AddRange(receivedData);
                    return;
                }
                int signalFlag = (int)char.GetNumericValue(Convert.ToChar(receivedData[0]));
                if (signalFlag == (int)SignalFlag.COMMAND)
                {
                    string resourcePath = byteToString(receivedData);
                    Debug.Log("Command " + resourcePath);

                    texture_loader.setTexture(texture_loader.loadResource(resourcePath));
                }
                else if (signalFlag == (int)SignalFlag.FILENAME)
                {
                    string filename = byteToString(receivedData);
                    Debug.Log("Filename " + filename);

                    texture_loader.setTexture(texture_loader.loadTexture(
                        Application.persistentDataPath + filename, resolution));
                }
                else if (signalFlag == (int)SignalFlag.TEXTURE)
                {
                    isReadingTexture = true;
                    readingCounter = 0;
                    textureFileName = byteToString(receivedData);
                    Debug.Log("Texture " + textureFileName);
                }
            }
        }
        catch (TimeoutException)
        {
            //do nothing
            if (isReadingTexture&&readingCounter>MAX_READ)
            {
                Debug.Log("Done reading");
                isReadingTexture = false;
                readingCounter = 0;
                Debug.Log(textureBytes.Count);
                texture_loader.setTexture(texture_loader.loadFromBytes(textureBytes.ToArray(), resolution));
                Debug.Log(Application.persistentDataPath + "/" + textureFileName);
                Debug.Log(byteToString(textureBytes.ToArray()));
                File.WriteAllBytes(Application.persistentDataPath + "/" + textureFileName, textureBytes.ToArray());

            } else if(isReadingTexture&&readingCounter<=MAX_READ)
            {
                readingCounter++;
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
