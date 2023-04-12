using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private int health;
    private int numOfHearts;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHearth;
    [SerializeField] private Sprite emptyHearth;

    private GameManager GM;

    private PlayerController player;

    private void Start() {
        health = numOfHearts;
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update() {
        health = player.health;
        numOfHearts = player.numOfHearts;

        if(health > numOfHearts) {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health) {
                hearts[i].sprite = fullHearth;
            } else {
                hearts[i].sprite = emptyHearth;
            }

            if(i > numOfHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }

}
