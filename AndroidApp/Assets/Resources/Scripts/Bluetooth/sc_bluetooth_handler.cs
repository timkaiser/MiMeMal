using UnityEngine;
using System;

public class sc_bluetooth_handler : MonoBehaviour
{
    public enum SignalFlag{
        COMMAND, FILENAME, TEXTURE
    };

    public string pcName;

    private static sc_bluetooth_handler bluetoothHandler = null;
    private AndroidJavaObject btplugin = null;

    private void Awake()
    {
        if(bluetoothHandler!=null && bluetoothHandler!=this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            bluetoothHandler = this;
        }

        if (btplugin == null)
        {
            try
            {
                using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.mptum.bluetoothplugin.BTPlugin"))
                {
                    if (pluginClass != null)
                    {
                        btplugin = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                        btplugin.Call("init", pcName);
                        bool connected = btplugin.Call<bool>("connect");
                        Debug.Log("connected: " + connected);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Could not establish bluetooth connection! " + e.Message);
            }
        }
    }

    private void OnDisable()
    {
        try
        {
            btplugin.Call("close");
        } catch (Exception e)
        {
            Debug.LogError("Could not close bluetooth connection! " + e.Message);
        }
    }

    public static sc_bluetooth_handler getInstance()
    {
        return bluetoothHandler;
    }

    public bool send(String message, SignalFlag flag)
    {
        if (btplugin != null)
        {
            String m = (int)flag + message;
            try
            {
                Debug.Log("Sending: " + m);
                return btplugin.Call<bool>("sendText", m);
            } catch (Exception e)
            {
                Debug.LogError("Could not send message! " + e.Message);
            }
        }
        return false;
    }

    public bool sendTexture(byte[] data)
    {
        if (btplugin != null)
        {
            try
            {
                return btplugin.Call<bool>("send", data);
            }
            catch (Exception e)
            {
                Debug.LogError("Could not send texture! " + e.Message);
            }
        }
        return false;
    }
}