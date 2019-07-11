using UnityEngine;
using System;

public class sc_bluetooth : MonoBehaviour
{
    private AndroidJavaObject btplugin = null;
    private AndroidJavaObject activityContext = null;

    void Start()
    {
        if (btplugin == null)
        {
            try
            {
                using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                }
                using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.mptum.bluetoothplugin.BTPlugin"))
                {
                    if (pluginClass != null)
                    {
                        btplugin = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                        btplugin.Call("setContext", activityContext);
                        String result = btplugin.Call<String>("init");
                        Debug.Log(result);
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

}