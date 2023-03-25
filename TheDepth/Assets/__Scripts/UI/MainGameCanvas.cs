using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameCanvas : SingletonMonobehaviour<MainGameCanvas>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
