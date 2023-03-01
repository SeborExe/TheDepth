using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private bool invertXAxis = false;
    [SerializeField] private bool invertYAxis = true;

    private CinemachineFreeLook freeLookCamera;

    private void Awake()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();

        freeLookCamera.m_XAxis.m_InvertInput = invertXAxis;
        freeLookCamera.m_YAxis.m_InvertInput = invertYAxis;
    }

    private void Update()
    {
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
    }

    void LateUpdate()
    {
        transform.LookAt(2 * transform.position - freeLookCamera.transform.position);
    }
}
