using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public AnimatorOverrideController animatorOverride;
    public WeaponPrefabSO weaponPrefab;
    public float minDamage;
    public float maxDamage;
    public AttackSO[] attack;
}
