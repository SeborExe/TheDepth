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
        LoadLastScene();
    }

    private IEnumerator LoadLastScene()
    {
        yield return savingSystem.LoadLastScene(dafaulcSaveFile);
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

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Delete();
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

    public void Delete()
    {
        savingSystem.Delete(dafaulcSaveFile);
    }
}
