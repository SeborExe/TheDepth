using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [Header("All")]
    public WeaponPrefabSO weaponPrefab;
    public bool isDistanceWeapon = false;
    public bool isRightHanded = true;
    public float minDamage;
    public float maxDamage;

    [Header("Melee")]
    public AnimatorOverrideController animatorOverride;
    public AttackSO[] attack;

    [Header("Distance")]
    public ProjectileSO projectileSO;
    public float knockback;
    public float force;
    public bool hasImpact;
}
