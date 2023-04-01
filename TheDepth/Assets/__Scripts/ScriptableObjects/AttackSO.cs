using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack_", menuName = "Weapon/AttackSO")]
public class AttackSO : ScriptableObject
{
    public string AnimationName;
    public float TransitionDuration;
    public int ComboStateIndex = -1;
    public float ComboAttackTime;
    public float ForceTime;
    public float Force;
    public int Knockback;
    public bool HasImpact;
}
