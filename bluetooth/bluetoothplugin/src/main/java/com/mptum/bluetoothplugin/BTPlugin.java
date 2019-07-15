package com.mptum.bluetoothplugin;

import android.bluetooth.*;
import android.content.Context;
import android.util.Log;

import java.io.IOException;
import java.io.OutputStream;
import java.util.Set;
import java.util.UUID;

public class BTPlugin {
    private static BTPlugin instance;
    private BluetoothDevice device;
    private BluetoothSocket socket;
    private OutputStream os;

    public BTPlugin()
    {
        this.instance = this;
    }

    public static BTPlugin getInstance()
    {
        if(instance==null)
        {
            instance = new BTPlugin();
        }
        return instance;
    }

    public void init(String deviceName)
    {
        BluetoothAdapter BTAdapter = BluetoothAdapter.getDefaultAdapter();
        Set<BluetoothDevice> pairedDevices = BTAdapter.getBondedDevices();

        for(BluetoothDevice d : pairedDevices){
            if(d.getName().equalsIgnoreCase(deviceName)) {
                device = d;
            }
        }
    }

    public boolean connect()
    {
        try {
            UUID uuid = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");
            socket = device.createRfcommSocketToServiceRecord(uuid);
            socket.connect();
            os = socket.getOutputStream();
        } catch (Exception e) {
            try {
                socket.close();
            } catch (IOException ec) {
                return false;
            }
            return false;
        }
        return true;
    }

    public boolean send(byte[] bytes)
    {
        try {
            os.write(bytes);
        } catch (Exception e) {
            return false;
        }
        return true;
    }

    public boolean sendText(String s)
    {
        try {
            os.write(s.getBytes());
        } catch (Exception e) {
            return false;
        }
        return true;
    }

    public boolean close()
    {
        try {
            socket.close();
        } catch (Exception e) {
            return false;
        }
        return true;
    }
}
