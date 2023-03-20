using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Scenes targetScene;

    public static void Load(Scenes scene)
    {
        targetScene = scene;
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadSceneAsync(targetScene.ToString());
    }
}
