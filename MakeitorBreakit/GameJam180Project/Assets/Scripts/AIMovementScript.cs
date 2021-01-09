using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovementScript : MonoBehaviour
{
    public  Transform destination;

    Vector3 lastLocation;

    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        lastLocation = destination.position;

        if (navMeshAgent == null)
        {
            Debug.Log("no nav mesh");
        } else
        {
            SetDestination();
        }
    }

    public void Update()
    {
        if (lastLocation != destination.position)
        {
            lastLocation = destination.position;
            SetDestination();
        }
    }

    public void SetDestination()
    {
        Vector3 targetVector = destination.transform.position;
        navMeshAgent.SetDestination(targetVector);
    }
}
