using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*HEADER:
 * This is a script used for expermiments. It calls the cs_lineDrawer compute shader that dras a straight line between to points onto a RenderTexture.
 * The result is saved in RenderTexture rt and can be accessed via the Unity Inspector.
 */

public class sc_drawLine : MonoBehaviour
{
    //Rendertexture for Output
    public RenderTexture rt;

    void Start() {
        //Initialize RenderTexture
        rt = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
        rt.enableRandomWrite = true;
        rt.filterMode = FilterMode.Point;
        rt.anisoLevel = 0;
        rt.Create();

        //Initilaize compute shader
        ComputeShader cs_drawline = (ComputeShader)Resources.Load("Shader/cs_lineDrawer");
        int csKernel = cs_drawline.FindKernel("CSMain");

        //call compute shader
        cs_drawline.SetTexture(csKernel, "Result", rt);
        cs_drawline.Dispatch(csKernel, rt.width/8, rt.height/8, 1);

    }
    
}
