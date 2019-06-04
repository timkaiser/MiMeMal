using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_draw : MonoBehaviour {
    RenderTexture canvas;
    int canvas_size = 512;

    //brush
    Texture2D brush_positionMap;

    [SerializeField]
    Texture2D brush_stencil;

    [SerializeField]
    int brush_size = 50;

    [SerializeField]
    GameObject object_focused;

    [SerializeField]
    public Color drawing_color;

    [SerializeField]
    Texture2D texture;
    //compute shader
    ComputeShader cs_draw;
    private int csKernel;

    public Camera cam;

    [SerializeField]
    Texture2D component_mask;

    [SerializeField]
    float component_id = -1;

    //old mouse position
    float mouse_x_old = -1;
    float mouse_y_old = -1;

    // Use this for initialization
    
	
	// Update is called once per frame
	void Update () {
        
    }

    
    
}

