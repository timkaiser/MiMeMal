using UnityEngine;

public class sc_outlineprojection : MonoBehaviour {
    [SerializeField]
    RenderTexture result;
    [SerializeField]
    int result_size = 2048;


    [SerializeField]
    RenderTexture uv_image;

    [SerializeField]
    Texture2D outline_projection;

    //compute shader
    ComputeShader cs_projection;
    private int csKernel;
    
    // Use this for initialization
    void Start() {
        result = new RenderTexture(result_size, result_size, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        result.enableRandomWrite = true;
        result.filterMode = FilterMode.Point;
        result.anisoLevel = 0;
        result.Create();

        //setup compute shader
        cs_projection = (ComputeShader)Resources.Load("Shader/cs_projection");
        csKernel = cs_projection.FindKernel("projectOutline");
    }

    int timer = 10;
    // Update is called once per frame
    void Update() {
        timer--;
        if (timer == 0) {
            uv_image = sc_UVCamera.uv_image;
            cs_projection.SetTexture(csKernel, "Result", result);
            cs_projection.SetTexture(csKernel, "UV", sc_UVCamera.uv_image);
            cs_projection.SetTexture(csKernel, "Outline", outline_projection);

            cs_projection.SetInt("scalefactor", sc_UVCamera.scale_factor);

            Debug.Log(sc_UVCamera.uv_image.height+"|"+ sc_UVCamera.uv_image.width+" = "+ outline_projection.height + "|" + outline_projection.width);

            cs_projection.Dispatch(csKernel, sc_UVCamera.uv_image.width / 8, sc_UVCamera.uv_image.height / 8, 1);

            SaveRTToFile(result);
        }
    }

    public static void SaveRTToFile(RenderTexture rt) { //source https://gist.github.com/krzys-h/76c518be0516fb1e94c7efbdcd028830
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        bytes = tex.EncodeToPNG();

        string path = "Assets/outlines.png";
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log("Saved to " + path);
    }


}

