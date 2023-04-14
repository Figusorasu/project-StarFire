using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text debugInfoText;

    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {

        float pPos_x = player.GetComponent<Transform>().position.x;
        float pPos_y = player.GetComponent<Transform>().position.y;

        pPos_x = (float)System.Math.Round(pPos_x, 2);
        pPos_y = (float)System.Math.Round(pPos_y, 2);

        float pVel_x = player.GetComponent<PlayerController>()._rb.velocity.x;
        float pVel_y = player.GetComponent<PlayerController>()._rb.velocity.y;

        pVel_x = (float)System.Math.Round(pVel_x, 2);
        pVel_y = (float)System.Math.Round(pVel_y, 2);

        var debugLog = "";

        debugLog += "FPS: " + CurrentFPS() + "\n\n";

        debugLog += "PLAYER:\n";
        debugLog += "Position: " + pPos_x + "(x), " + pPos_y + "(y);\n";
        debugLog += "Velocity: " + pVel_x + "(x), " + pVel_y + "(y);\n";
        debugLog += "IsGrounded: " + player.GetComponent<PlayerController>().isGrounded + "\n";
        debugLog += "";

        debugInfoText.text = debugLog;
    }

    private string CurrentFPS() {
        float frameRate = 0;
        frameRate = Time.frameCount / Time.time;
        return "" + (int)frameRate;
    }
}