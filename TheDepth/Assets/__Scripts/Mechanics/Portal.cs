using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private Scenes scene;
    [SerializeField] private DestinationIdentifier destination;
    [SerializeField] private Animator FadeScreenAnimator;
    [SerializeField] private Transform spawnPoint;

    private int timeToChangeSceen = 1;
    private readonly int FADEOUT = Animator.StringToHash("Fadeout");

    private async void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (FadeScreenAnimator != null) { FadeScreenAnimator.SetBool(FADEOUT, true); }

            await Load();
        }
    }

    private async Task Load()
    {
        await Task.Delay(timeToChangeSceen * 1000);

        DontDestroyOnLoad(gameObject);

        Loader.Load(scene);

        SceneManager.LoadSceneAsync(Scenes.LoadingScene.ToString());

        while (SceneManager.GetActiveScene().name != scene.ToString())
        {
            await Task.Yield();
        }

        await Task.Yield();

        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);
        Destroy(gameObject);
    }

    private Portal GetOtherPortal()
    {
        foreach(Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.destination != destination) continue;

            return portal;
        }

        return null;
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        Player player = Player.Instance;
        player.gameObject.transform.position = otherPortal.spawnPoint.position;
        player.gameObject.transform.rotation = otherPortal.spawnPoint.rotation;
    }
}
