using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform Player;
    public GameManagerScript gameManager;
    public Camera gameCamera;

    public bool inSight = false;

    public float spawnTimer = 10f;

    private void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    public void Update()
    {
        CheckIfCameraCanSee();
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemy.GetComponent<AIMovementScript>().destination = Player;
        enemy.GetComponent<AIBrainScript>().player = Player.gameObject;
        enemy.GetComponent<TargetScript>().gameManager = this.gameManager;
    }

    public void CheckIfCameraCanSee()
    {
        Vector3 screenPoint = gameCamera.WorldToViewportPoint(transform.position);

        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            inSight = true;
        }
        else
        {
            inSight = false;
        }
    }

    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(spawnTimer);
        if (!inSight)
        {
            SpawnEnemy();
        }
        StartCoroutine(SpawnTimer());
    }
}
