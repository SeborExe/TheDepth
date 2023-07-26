using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInfoPanelUI : SingletonMonobehaviour<NPCInfoPanelUI>
{
    [SerializeField] private TMP_Text infoText;

    protected override void Awake()
    {
        base.Awake();

        gameObject.SetActive(false);
    }

    public void Initialize(string text)
    {
        infoText.text = text;
        gameObject.SetActive(true);
    }
}
