using UnityEngine;

/* HEADER:
 * This script has to be atached to a secondary camera.
 * It renders the scene with UV coordinates using the sh_UVShader as replacement shader.
 * The result is stored in a RenderTexture.
 */
[RequireComponent(typeof(Camera))]
public class sc_projector_camera : MonoBehaviour {

    void Start() {      
        GetComponent<Camera>().SetReplacementShader((Shader)Resources.Load("Shader/sh_projector"), "");
    }
}
