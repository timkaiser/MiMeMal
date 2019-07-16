using System.Collections;
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

        public CalibrationData(Matrix4x4 m) {
            matrix = new float[16];
            for(int i=0; i<16; i++) {
                matrix[i] = m[i];
            }
        }
    }

    //saves settings
    public static void save(Matrix4x4 matrix) {
        CalibrationData data = new CalibrationData(matrix);

        string destination = Application.persistentDataPath + "/calibration.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    //loads settings
    public static Matrix4x4 load() {
        string destination = Application.persistentDataPath + "/calibration.dat";
        FileStream file;
        Matrix4x4 result = new Matrix4x4();

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else {
            result = Matrix4x4.identity;
            result.m22 = -1;
            result.m23 = -5;
            return result;
        }

        BinaryFormatter bf = new BinaryFormatter();
        CalibrationData data = (CalibrationData)bf.Deserialize(file);
        file.Close();

        for (int i = 0; i < 16; i++) {
            result[i] = data.matrix[i];
        }

        return result;
    }
}
