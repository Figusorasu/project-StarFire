using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHearth;
    [SerializeField] private Sprite emptyHearth;

    private GameManager GM;

    private void Start() {
        health = maxHealth;
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    private void Update() {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }

}
