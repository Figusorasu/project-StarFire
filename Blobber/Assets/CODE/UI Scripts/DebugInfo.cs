using UnityEngine;
using TMPro;

public class DebugInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text debugInfoText;

    private GameObject player;

    private float pPos_x;
    private float pPos_y;
    private float pVel_x;
    private float pVel_y;
    private float yLastVelocity = 0;
    private float yMaxVelocity;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        pPos_x = (float)System.Math.Round(player.GetComponent<Transform>().position.x, 2);
        pPos_y = (float)System.Math.Round(player.GetComponent<Transform>().position.y, 2);

        pVel_x = (float)System.Math.Round(player.GetComponent<PlayerController>()._rb.velocity.x, 2);
        pVel_y = (float)System.Math.Round(player.GetComponent<PlayerController>()._rb.velocity.y, 2);

        if(pVel_y > yLastVelocity) {
            yLastVelocity = pVel_y;
            yMaxVelocity = pVel_y;
        }

        var debugLog = "";

        debugLog += "FPS: " + CurrentFPS() + "\n";
        debugLog += "Time: " + System.Math.Round(Time.time, 2) + "\n Δ-time: " + Time.deltaTime + "\n";

        debugLog += "\n";

        debugLog += "PLAYER:\n";
        debugLog += "Position: " + pPos_x + "(x), " + pPos_y + "(y);\n";
        debugLog += "Velocity: " + pVel_x + "(x), " + pVel_y + "(y);\n";

        debugLog += "Max Y Velocity: " + (float)System.Math.Round(yMaxVelocity, 2) + "\n";

        debugLog += "IsGrounded: " + player.GetComponent<PlayerController>().isGrounded + "\n";
        debugLog += "InputHorizontal: " + player.GetComponent<PlayerController>().inputHorizontal + "\n";

        debugInfoText.text = debugLog;
    }

    private string CurrentFPS() {
        float frameRate = 0;
        frameRate = Time.frameCount / Time.time;
        return "" + (int)frameRate;
    }
}