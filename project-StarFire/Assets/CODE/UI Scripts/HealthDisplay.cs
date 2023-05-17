using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHearth;
    [SerializeField] private Sprite emptyHearth;

    private GameManager GM;

    private PlayerController player;

    private void Start() {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    private void Update() {
        if(player.health > player.numOfHearts) {
            player.health = player.numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < player.health) {
                hearts[i].sprite = fullHearth;
            } else {
                hearts[i].sprite = emptyHearth;
            }

            if(i < player.numOfHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }

}
