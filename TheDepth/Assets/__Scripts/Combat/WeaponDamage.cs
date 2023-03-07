using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private List<Collider> alreadyColliderWith = new List<Collider>();
    private float damage;

    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    public void Initialize(Mesh mesh)
    {
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
            health.Dealdamage(damage);
            alreadyColliderWith.Add(other);
        }
    }
    

    public void SetAttack(float damage)
    {
        this.damage = damage;
    }
}
