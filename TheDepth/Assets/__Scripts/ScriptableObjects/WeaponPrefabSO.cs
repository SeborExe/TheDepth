using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPrefab_", menuName = "Weapon/WeaponPrefabSO")]
public class WeaponPrefabSO : ScriptableObject
{
    public GameObject weaponGameObject;
    public Mesh weaponMesh;
}
