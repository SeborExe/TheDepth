using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private Scenes scene;
    [SerializeField] private Animator FadeScreenAnimator;

    private float timeToChangeSceen = 1f;
    private readonly int FADEOUT = Animator.StringToHash("Fadeout");

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        FadeScreenAnimator.SetBool(FADEOUT, true);
        Invoke(nameof(Load), timeToChangeSceen);
    }

    private void Load()
    {
        Loader.Load(scene);
    }
}
