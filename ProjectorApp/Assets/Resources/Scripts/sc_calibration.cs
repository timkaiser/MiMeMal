using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class sc_calibration : MonoBehaviour {
    // Keycodes _______________________________________________
    // Keycodes for calibration
    KeyCode[] key_codes = {
        KeyCode.RightArrow, KeyCode.LeftArrow,  // x-axis calibration
        KeyCode.UpArrow, KeyCode.DownArrow,     // y-axis calibration
        KeyCode.P, KeyCode.M                    // z-axis calibration
    };

    // Other keycodes
    KeyCode key_mode = KeyCode.B;       // change mainpulation mode
    KeyCode key_stepSize = KeyCode.N;   // change step sizes
    KeyCode key_toogle_ui = KeyCode.I;  // toogle debug UI

    // calibration setting __________________________
    // step sizes
    public float[] step_sizes = { 0.1f, 1.0f, 5.0f, 0.01f };
    public int current_step_size = 0;

    // modes
    public string[] modes = { "translate", "prespective", "scale" };
    public int current_mode = 0;

    public bool show_ui = true; // indicates wether to show the debug ui

    // vectors thant indicate what changes in different modes
    public Vector4[][] manipulation_vectors = {
         //translate
        new Vector4[]{
            new Vector4(0, 0, 0, 1),
            new Vector4(0, 0, 0, 1),
            new Vector4(0, 0, 0, 1)
        },
        //perspective
        new Vector4[]{
            new Vector4(0, 0, 1, -8),
            new Vector4(0, 0, 1, -7),
            new Vector4(0, 0, 1, -29)
        },
        //scale
        new Vector4[]{
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, 1, 0)
        }
    };

    public Matrix4x4 matrix;    // Projection matrix
    private Camera cam;    // Main Camera

    float time_since_last_change = -10;

    // Called on Startup
    public void Start() {
        cam = this.GetComponent<Camera>();              // get main camera
        matrix = sc_save_management.loadCalibration();//cam.worldToCameraMatrix;    // get projection matrix
        Screen.fullScreen = true;
    }

    // Update is called once per frame
    void Update() {
        // change mode
        if (Input.GetKeyDown(key_mode)) {
            current_mode = (current_mode + 1) % modes.Length;
        }
        // change step size
        if (Input.GetKeyDown(key_stepSize)) {
            current_step_size = (current_step_size + 1) % step_sizes.Length;
        }
        // toggle ui
        if (Input.GetKeyDown(key_toogle_ui)) {
            show_ui = !show_ui;
        }
        // save calibration
        if (time_since_last_change+3 < Time.time) {
            time_since_last_change = -10;
            sc_save_management.saveCalibration(matrix);
        }
        

        // manipulate projection matrix on input
        Vector4[] new_proj_vec = new Vector4[3];

        for (int i = 0; i < 3; i++) {
            if (Input.GetKeyDown(key_codes[i * 2])) {
                new_proj_vec[i] = matrix.GetRow(i) + manipulation_vectors[current_mode][i] * step_sizes[current_step_size];
            } else if (Input.GetKeyDown(key_codes[i * 2 + 1])) {
                new_proj_vec[i] = matrix.GetRow(i) + manipulation_vectors[current_mode][i] * step_sizes[current_step_size] * -1;
            } else {
                new_proj_vec[i] = matrix.GetRow(i);
            }
        }

        // apply manipulation
        matrix = new Matrix4x4(new_proj_vec[0], new_proj_vec[1], new_proj_vec[2], matrix.GetRow(3)).transpose;

        //remeber time for saving
        if(matrix != cam.worldToCameraMatrix) {
            time_since_last_change = Time.time;
        }

        cam.worldToCameraMatrix = matrix;
    }

}
