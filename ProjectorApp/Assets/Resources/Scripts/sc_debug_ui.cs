using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

 
public class sc_debug_ui : MonoBehaviour {
    public bool showFPS = true;     // Controls wether or not to show the FPS counter. Accessible via Unity Inspector.
    public bool limitFPS = false;    // Controls wether or not to limit the FPS. Accessible via Unity Inspector.
    public int maxFPS = 20;         // Max limit for the FPS. Accessible via Unity Inspector.

    GUIStyle style;                 // Visual appearance of the FPS counter

    public sc_calibration calibration_script;

    //called on startup
    private void Start() {
        //limit FPS is necessary
        if (limitFPS) {
            Application.targetFrameRate = maxFPS;
        }

        //set GUI style
        style = new GUIStyle();
        style.fontSize = Screen.height / 40;
        style.normal.textColor = Color.yellow;
    }



    // Called on GUI rendering
    void OnGUI() {
        if (!calibration_script.show_ui) { return; }

        StringBuilder msg = new StringBuilder("", 135);
        msg.Append("connected: ").Append(sc_connection_handler.instance.connected).AppendLine();
        msg.Append((int)(1 / Time.deltaTime)).Append(" FPS\t")
            .Append("Mode: ").Append(calibration_script.modes[calibration_script.current_mode])
            .Append("  Step size: ").Append(calibration_script.step_sizes[calibration_script.current_step_size])
            .AppendLine();

        if (calibration_script.current_mode < 3) {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    bool manipulated = i < 3 && calibration_script.manipulation_vectors[calibration_script.current_mode][i][j] != 0;

                    float m_ij = Mathf.Round(calibration_script.matrix.GetRow(i)[j] * 100) / 100.0f;

                    msg.Append(m_ij >= 0 ? " " : "")
                        .Append(manipulated ? "(" : " ")
                        .Append(m_ij)
                        .Append(manipulated ? ")" : " ")
                        .Append("\t");
                }
                msg.AppendLine();
            }
        } else {
            msg.Append(calibration_script.stele.GetComponent<Renderer>().material.GetFloat("_TextureBlendValue"));
            msg.AppendLine();
        }

        //Display FPS counter
        if (showFPS) {
            GUI.Label(new Rect(50, 50, 100, 100), msg.ToString(), style);
        }
    }
}