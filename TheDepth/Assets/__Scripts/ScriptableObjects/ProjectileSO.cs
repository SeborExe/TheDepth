using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile_", menuName = "Weapon/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public Projectile projectile;
    public float damage;
    public float knockback;
    public float force;
}
