using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class AimCameraController : SingletonMonobehaviour<AimCameraController>
{
    [SerializeField] private GameObject followTransform;
    [SerializeField] private float rotationPower = 3f;
    [SerializeField] private float rotationLerp = 0.5f;
    [SerializeField] private float speed = 1f;

    private InputHandler inputHandler;

    private Vector2 move;
    private Vector2 look;
    private bool _aim;

    private Vector3 nextPosition;
    private Quaternion nextRotation;

    protected override void Awake()
    {
        base.Awake();

        inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        move = inputHandler.GetMovementVectorNormalized();
        look = inputHandler.GetLookRotation();

        #region Player Based Rotation

        //Move the player based on the X input on the controller
        if (_aim)
        {
            transform.rotation *= Quaternion.AngleAxis(look.x * rotationPower, Vector3.up);
        }

        #endregion

        #region Follow Transform Rotation

        //Rotate the Follow Target transform based on the input
        followTransform.transform.rotation *= Quaternion.AngleAxis(look.x * rotationPower, Vector3.up);

        #endregion

        #region Vertical Rotation
        followTransform.transform.rotation *= Quaternion.AngleAxis(look.y * rotationPower, Vector3.left);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        followTransform.transform.localEulerAngles = angles;
        #endregion

        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        if (move.x == 0 && move.y == 0)
        {
            nextPosition = transform.position;

            if (_aim)
            {
                //Set the player rotation based on the look transform
                transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
                //reset the y rotation of the look transform
                followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }

            return;
        }

        float moveSpeed = speed / 100f;
        Vector3 position = (transform.forward * move.y * moveSpeed) + (transform.right * move.x * moveSpeed);
        nextPosition = transform.position + position;


        //Set the player rotation based on the look transform
        if (_aim)
        {
            transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        }

        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }

    public void SetMove(Vector2 _move)
    {
        this.move = _move;
    }

    public void SetLook(Vector2 _look)
    {
        this.look = _look;
    }

    public void SetAim(bool _aim)
    {
        this._aim = _aim;
    }
}
