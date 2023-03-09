using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReciver : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] private NavMeshAgent agent;

    private float verticalVelocity;
    private Vector3 impact;
    private Vector3 dampingVelocity;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    [SerializeField] private float drag = 0.1f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime * 10f;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (impact.sqrMagnitude < 0.2f * 0.2f && agent != null)
        {
            impact = Vector3.zero;
            agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
        {
            agent.enabled = false;
        }
    }
}
