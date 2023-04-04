using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingUI : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;

    private void Start()
    {
        ChangeCrosshairVisibility(false);
    }

    public void ChangeCrosshairVisibility(bool show)
    {
        crosshair.SetActive(show);
    }
}
