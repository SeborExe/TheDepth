using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private List<Collider> alreadyColliderWith = new List<Collider>();
    private float damage;
    private int knockback;
    private bool hasImpact;

    private float additionalDamage;

    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    public void Initialize(Mesh mesh, float additionalDamage)
    {
        this.additionalDamage = additionalDamage;

        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();

        meshCollider.sharedMesh = mesh;
        meshFilter.sharedMesh = mesh;
    }

    private void OnEnable()
    {
        alreadyColliderWith.Clear();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) { return; }

        if (alreadyColliderWith.Contains(other)) { return; }

        if (other.TryGetComponent(out Health health))
        {
            health.Dealdamage(damage, myCollider.gameObject, other.ClosestPoint(transform.position), hasImpact);
            alreadyColliderWith.Add(other);
        }

        if (other.TryGetComponent(out ForceReciver forceReciver))
        {
            forceReciver.AddForce((other.transform.position - myCollider.transform.position).normalized * knockback);
        }
    }
    

    public void SetAttack(float damage, int knockback, bool hasImpact)
    {
        this.damage = damage + additionalDamage;
        this.knockback = knockback;
        this.hasImpact = hasImpact;
    }
}
