using UnityEngine;

/* HEADER:
 * This script has to be atached to a secondary camera.
 * It renders the scene with UV coordinates using the sh_UVShader as replacement shader.
 * The result is stored in a RenderTexture.
 */
[RequireComponent(typeof(Camera))]
public class sc_UVCamera : MonoBehaviour {

    // This RenderTexture stores the results containing the UV coordinates
    public static RenderTexture uv_image;

    // This scalefactor is used to render the scene in a higher resolution 
    public static int scale_factor = 1;

    void Start () {
        //setup render target
        uv_image = new RenderTexture(GetComponent<Camera>().pixelWidth* scale_factor, GetComponent<Camera>().pixelHeight* scale_factor, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
        uv_image.enableRandomWrite = true;
        uv_image.filterMode = FilterMode.Point;
        uv_image.anisoLevel = 0;
        uv_image.Create();
        GetComponent<Camera>().SetTargetBuffers(uv_image.colorBuffer, uv_image.depthBuffer);

        //replacement shader
        GetComponent<Camera>().SetReplacementShader((Shader)Resources.Load("Shader/sh_UVShader"), "");
    }
}
