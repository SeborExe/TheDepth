using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, ISaveable
{
    public event Action<GameObject> OnTakeDamage;
    public event Action OnDie;

    [SerializeField] private float maxHealth = 100f;

    public bool IsDead => health == 0;

    private AnimatorController animatorController;

    private float health;
    private Vector3 attackPosition;
    private GameObject ragdoll;

    private void Awake()
    {
        animatorController = GetComponentInChildren<AnimatorController>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void Dealdamage(float damage, GameObject sender, Vector3 attackPosition)
    {
        if (health <= 0) { return; }
        if (animatorController.IsImmune) { return; }
        if (gameObject.tag == sender.tag) { return; }

        health = Mathf.Max(health - damage, 0);
        OnTakeDamage?.Invoke(sender);

        this.attackPosition = attackPosition;

        if (health == 0)
        {
            OnDie?.Invoke();
        }
    }

    public void InstantiateRagdoll(Transform ragdoll, WeaponSO currentWeapon)
    {
        Transform ragdollTransform = Instantiate(ragdoll, transform.position, transform.rotation);
        SetRagdollObjectWeapon(ragdollTransform, currentWeapon);

        MatchAllChildrenTransform(transform, ragdollTransform);
        ApplyExplosionToRagdoll(ragdollTransform, 500f, attackPosition, 10f);

        this.ragdoll = ragdollTransform.gameObject;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void SetRagdollObjectWeapon(Transform ragdollTransform, WeaponSO currentWeapon)
    {
        Transform weaponTransform =  ragdollTransform.GetComponent<Ragdoll>().weaponTransform;
        Instantiate(currentWeapon.weaponPrefab.weaponGameObject, weaponTransform);
    }

    private void MatchAllChildrenTransform(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildrenTransform(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosiion, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosiion, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosiion, explosionRange);
        }
    }

    public object CaptureState()
    {
        return health;
    }

    public void RestoreState(object state)
    {
        health = (float)state;
        if (health > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (ragdoll != null)
            {
                Destroy(ragdoll);
            }
        }
    }
}
