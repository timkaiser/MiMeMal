using UnityEngine;

[RequireComponent(typeof(Camera))]
public class sc_UVCamera : MonoBehaviour {

    public static RenderTexture uv_image;
    public static int scale_factor = 4;
    Shader uv_shader;

    void Start () {
        //Setup camera
        Camera cam = this.GetComponent<Camera>();

        //setup render target
        uv_image = new RenderTexture(cam.pixelWidth* scale_factor, cam.pixelHeight* scale_factor, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
        uv_image.enableRandomWrite = true;
        uv_image.filterMode = FilterMode.Point;
        uv_image.anisoLevel = 0;
        uv_image.Create();
        
        cam.SetTargetBuffers(uv_image.colorBuffer, uv_image.depthBuffer);

        //replacement shader
        uv_shader = (Shader)Resources.Load("Shader/sh_UVShader");
        cam.SetReplacementShader(uv_shader, "");

        
    }


}
