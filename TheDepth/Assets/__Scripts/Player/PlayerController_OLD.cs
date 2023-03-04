using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Mover mover;

    [field: SerializeField] public float PlayerMoveSpeed { get; private set; }

    private void Awake()
    {
        mover = GetComponent<Mover>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && mover.IsKeyboardMovementEnabled)
        {
            MoveToCursor();
        }
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit);
        if (hasHit)
        {
            mover.MoveTo(hit.point);
        }
    }
}
