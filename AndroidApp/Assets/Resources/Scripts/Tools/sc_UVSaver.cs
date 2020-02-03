using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class sc_UVSaver : MonoBehaviour
{
    public bool debug_save = false;
    // Start is called before the first frame update
    void Update()
    {
        if (debug_save) {
            saveUV();
            debug_save = false;
        }
    }

    void saveUV() {
        //convert rendertexture to texture
        Texture2D uv_image_tex;
        var oldRT = RenderTexture.active;
        uv_image_tex = new Texture2D(sc_UVCamera.uv_image.width, sc_UVCamera.uv_image.height, TextureFormat.RGBAFloat, false);
        RenderTexture.active = sc_UVCamera.uv_image;
        uv_image_tex.ReadPixels(new Rect(0, 0, sc_UVCamera.uv_image.width, sc_UVCamera.uv_image.height), 0, 0);
        uv_image_tex.Apply();
        RenderTexture.active = oldRT;

        //convert to byte
        byte[] data = uv_image_tex.GetRawTextureData();

        //save byte in file
        string name = "uv_" + sc_UVCamera.uv_image.width + "x" + sc_UVCamera.uv_image.height;
        string path = name;
        System.IO.File.WriteAllBytes(path, data);
        Debug.Log("File saved");
    }
}
