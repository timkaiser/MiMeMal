using System;
using UnityEngine;

/* HEADER:
 * This script has to be atached to a secondary camera.
 * It renders the scene with UV coordinates using the sh_UVShader as replacement shader.
 * The result is stored in a RenderTexture.
 */
[RequireComponent(typeof(Camera))]
public class sc_UVCamera : MonoBehaviour
{

    // This RenderTexture stores the results containing the UV coordinates
    public static RenderTexture uv_image;
    public static Texture2D uv_image_tex = null;

    // This scalefactor is used to render the scene in a higher resolution 
    public static int scale_factor = 1;

    void Start()
    {
        //setup render target
        uv_image = new RenderTexture(GetComponent<Camera>().pixelWidth * scale_factor, GetComponent<Camera>().pixelHeight * scale_factor, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
        uv_image.enableRandomWrite = true;
        uv_image.filterMode = FilterMode.Point;
        uv_image.anisoLevel = 0;
        uv_image.Create();

        GetComponent<Camera>().SetTargetBuffers(uv_image.colorBuffer, uv_image.depthBuffer);

        //replacement shader
        GetComponent<Camera>().SetReplacementShader((Shader)Resources.Load("Shader/sh_UVShader"), "");
    }

    public static void update_texture()
    {
        if (uv_image_tex != null) return;

        var oldRT = RenderTexture.active;
        uv_image_tex = new Texture2D(uv_image.width, uv_image.height, TextureFormat.RGBAFloat, false);
        RenderTexture.active = uv_image;
        uv_image_tex.ReadPixels(new Rect(0, 0, uv_image.width, uv_image.height), 0, 0);
        uv_image_tex.Apply();
        RenderTexture.active = oldRT;

        if(!sc_connection_handler.instance.send_uvimage(uv_image_tex, new Vector2(uv_image.width, uv_image.height))) uv_image_tex = null;
    }
}
