﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// this script offers static save and load methodes to save a Matrix4x4 to "calibration.dat"
public class sc_save_management : MonoBehaviour {
    //struct for saving the calibration
    [System.Serializable]
    struct CalibrationData{
        public float[] matrix;
        public float colorBlend;
        public CalibrationData(Matrix4x4 m, float colorBlend) {
            matrix = new float[16];
            for(int i=0; i<16; i++) {
                matrix[i] = m[i];
            }
            this.colorBlend = colorBlend;
        }
    }

    //saves settings
    public static void saveCalibration(Matrix4x4 matrix, float colorBlend) {
        CalibrationData data = new CalibrationData(matrix, colorBlend);

        string destination = Application.persistentDataPath + "/calibration.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    //loads settings
    public static void loadCalibration(out Matrix4x4 result, out float colorBlend) {
        string destination = Application.persistentDataPath + "/calibration.dat";
        FileStream file;
        result = new Matrix4x4();

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else {
            result = Matrix4x4.identity;
            result.m22 = -1;
            result.m23 = -5;
            colorBlend = 0.16f;
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        CalibrationData data = (CalibrationData)bf.Deserialize(file);
        file.Close();

        for (int i = 0; i < 16; i++) {
            result[i] = data.matrix[i];
        }

        colorBlend = data.colorBlend;
    }

    //loads settings
    public static void loadNetConfig(out string ip, out string port) {
        string destination = Application.persistentDataPath + "/netconfig.txt";

        try {
            StreamReader reader = new StreamReader(destination);
            ip = reader.ReadLine();
            port = reader.ReadLine();
            reader.Close();
        } catch {
            StreamWriter writer = new StreamWriter(destination, false);
            writer.WriteLine("localhost");
            writer.WriteLine("8101");
            writer.Close();

            ip = "localhost";
            port = "8101";

            Debug.LogError("netconfig.txt not found. Loading default configuration (loacalhost:8101)");
        }
    }
}
