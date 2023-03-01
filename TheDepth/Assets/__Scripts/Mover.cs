using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private Transform target;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
}
