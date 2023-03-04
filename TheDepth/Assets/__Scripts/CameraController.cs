using Cinemachine;
using UnityEngine;

public class CameraController : SingletonMonobehaviour<CameraController>
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float gamepadRotateSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private bool invertXAxis = false;
    [SerializeField] private bool invertYAxis = true;

    private CinemachineFreeLook freeLookCamera;

    private Vector2 vectorRotation;
    private float zoomValue;

    protected override void Awake()
    {
        base.Awake();

        freeLookCamera = GetComponent<CinemachineFreeLook>();

        freeLookCamera.m_XAxis.m_InvertInput = invertXAxis;
        freeLookCamera.m_YAxis.m_InvertInput = invertYAxis;
    }

    private void Update()
    {  
        freeLookCamera.m_XAxis.Value = vectorRotation.x * gamepadRotateSpeed;

        if (zoomValue != 0)
        {
            freeLookCamera.m_YAxis.Value += zoomValue  * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        transform.LookAt(2 * transform.position - freeLookCamera.transform.position);
    }

    public void SetVectorRotation(Vector2 vectorRotation)
    {
        this.vectorRotation = vectorRotation;
    }

    public void SetZoomValue(float zoomValue)
    {
        this.zoomValue = zoomValue;
    }
}
