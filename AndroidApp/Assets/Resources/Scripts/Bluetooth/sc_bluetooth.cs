using UnityEngine;
using System;
using System.Threading;

public class sc_bluetooth : MonoBehaviour
{
    private AndroidJavaObject btplugin = null;
    private bool running = true;

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
                        String result = btplugin.Call<String>("init");
                        Debug.Log(result);
                        bool connected = btplugin.Call<bool>("connect");
                        Debug.Log("connected: " + connected);
                        Debug.Log(btplugin.Call<bool>("sendText", "Hello"));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Could not establish bluetooth connection!");
                Debug.LogError(e.Message);
            }
        }
    }

    private void OnDisable()
    {
        running = false;
        try
        {
            btplugin.Call("close");
        } catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}