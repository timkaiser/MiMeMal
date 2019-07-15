using UnityEngine;
using System;

public class sc_bluetooth_handler : MonoBehaviour
{
    public enum SignalFlag{
        COMMAND, FILENAME, TEXTURE
    };

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
    }

    private void Start()
    {
        if (btplugin == null)
        {
            try
            {
                using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.mptum.bluetoothplugin.BTPlugin"))
                {
                    if (pluginClass != null)
                    {
                        btplugin = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                        btplugin.Call("init", "SMAUG");
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
            Debug.Log(e.Message);
        }
    }

    public static sc_bluetooth_handler getInstance()
    {
        return bluetoothHandler;
    }

    public bool send(String message, SignalFlag flag)
    {
        char c = (char)flag;
        String m = c + message;
        return btplugin.Call<bool>("sendText", m);
    }
}