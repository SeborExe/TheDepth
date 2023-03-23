using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Scenes targetScene;

    private const int timeToWaitAfterLoadScene = 500;

    public static void Load(Scenes scene)
    {
        targetScene = scene;
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadSceneAsync(targetScene.ToString());
    }

    public static async Task LoadScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene.ToString());
        await Task.Delay(timeToWaitAfterLoadScene);
        await Task.Yield();
    }

    public static string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
