using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    [Header("Player Progression")]
    public bool unlockMultiJump = false;
    public bool unlockDash = false;
    public bool unlockStomp = false;


}
