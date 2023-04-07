using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Start() {
        float waitTime = Random.Range(1,5);

        StartCoroutine("startIdleAnim", waitTime);
    }

    IEnumerator startIdleAnim(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        anim.SetTrigger("Idle");
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            Debug.Log("kurwa");
            anim.SetTrigger("Collect");
            Debug.Log("kurwa2");
        }
    }


}