using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject debugInfo;
    private bool isActive = false;

    private void Update() {
        debugInfo.SetActive(isActive);

        if(Input.GetKeyDown(KeyCode.F3)) {
            isActive = !isActive;
        }
    }
}