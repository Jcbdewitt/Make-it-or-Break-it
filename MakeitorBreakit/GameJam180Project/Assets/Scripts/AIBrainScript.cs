using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrainScript : MonoBehaviour
{
    public GameObject player;

    public float distanceFromEnemy = 1.5f;
    public float timeInSight = 3f;

    public bool playerInSight = false;
    public bool timerStarted = false;

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distanceFromEnemy)
        {
            if (!timerStarted)
            {
                timerStarted = true;
                StartCoroutine(HitWaitTimer());
            }
        } else
        {
            if (timerStarted)
            {
                timerStarted = false;
                StopAllCoroutines();
            }
        }
    }


    IEnumerator HitWaitTimer()
    {
        yield return new WaitForSeconds(timeInSight);
        Debug.Log("Hit");
        player.GetComponent<PlayerMovementScript>().Hit();
    }
}
