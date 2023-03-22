using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingWrapper : SingletonMonobehaviour<SavingWrapper>
{
    private const string dafaulcSaveFile = "save";

    private SavingSystem savingSystem;

    protected override void Awake()
    {
        base.Awake();

        savingSystem = GetComponent<SavingSystem>();

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Save();
        }
    }

    public void Load()
    {
        savingSystem.Load(dafaulcSaveFile);
    }

    public void Save()
    {
        savingSystem.Save(dafaulcSaveFile);
    }
}
