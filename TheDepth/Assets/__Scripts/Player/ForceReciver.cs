using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReciver : MonoBehaviour
{
    private CharacterController characterController;

    private float verticalVelocity;
    public Vector3 Movement => Vector3.up * verticalVelocity;

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
    }
}
