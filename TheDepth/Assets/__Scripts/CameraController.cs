using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float gamepadRotateSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private bool invertXAxis = false;
    [SerializeField] private bool invertYAxis = true;

    private CinemachineFreeLook freeLookCamera;
    private Mover mover;

    private void Awake()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();

        freeLookCamera.m_XAxis.m_InvertInput = invertXAxis;
        freeLookCamera.m_YAxis.m_InvertInput = invertYAxis;

        mover = FindObjectOfType<Mover>();
    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(1))
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = rotateSpeed;
        }
        if (Input.GetMouseButtonUp(1))
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            freeLookCamera.m_YAxis.m_MaxSpeed = zoomSpeed;
        }
        */

        freeLookCamera.m_XAxis.Value = mover.GetLookRotation().x * gamepadRotateSpeed;

        if (mover.zoomValue != 0)
        {
            freeLookCamera.m_YAxis.Value += mover.zoomValue  * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        transform.LookAt(2 * transform.position - freeLookCamera.transform.position);
    }
}
