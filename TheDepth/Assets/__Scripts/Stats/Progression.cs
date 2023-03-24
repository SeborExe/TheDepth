using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Progression", menuName = "ProgressionSO")]
public class Progression : ScriptableObject
{
    [SerializeField] private List<ProgressionCharacterClass> characterClasses;

    private Dictionary<CharacterClass, ProgressionCharacterClass> characterClassDict;

    private void OnEnable()
    {
        characterClassDict = characterClasses.ToDictionary(x => x.characterClass, x => x);
    }

    [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
    }

    [System.Serializable]
    class ProgressionStat
    {
        public Stat stat;
        public float[] levels;
    }

    public float GetStat(Stat stat, CharacterClass characterClass, int level)
    {
        if (!characterClassDict.ContainsKey(characterClass)) return 1;

        var progressionClass = characterClassDict[characterClass];

        var progressionStat = progressionClass.stats.FirstOrDefault(x => x.stat == stat && x.levels.Length >= level);
        if (progressionStat == null) return 1;

        return progressionStat.levels[level - 1];
    }
}
