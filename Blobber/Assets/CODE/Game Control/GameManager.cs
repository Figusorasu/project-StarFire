using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int targetedFramerate;

    private static GameManager instance;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 60;
    }

    private void Update() {
        if(targetedFramerate != 0) {
            Application.targetFrameRate = targetedFramerate;
        }
    }

    [Header("Player Progression")]
    public bool unlockMultiJump = false;
    public bool unlockDash = false;
    public bool unlockStomp = false;


}
