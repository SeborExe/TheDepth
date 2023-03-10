using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDie;

    [SerializeField] private float maxHealth = 100f;

    public bool IsDead => health == 0;

    private AnimatorController animatorController;

    private float health;

    private void Awake()
    {
        animatorController = GetComponentInChildren<AnimatorController>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void Dealdamage(float damage, GameObject sender)
    {
        if (health <= 0) { return; }
        if (animatorController.IsImmune) { return; }
        if (gameObject.tag == sender.tag) { return; }

        health = Mathf.Max(health - damage, 0);
        OnTakeDamage?.Invoke();

        //if (TryGetComponent(out EnemyStateMachine enemyStateMachine))
        //{
        //    enemyStateMachine.ChangePlayerDetection(sender);
        //}

        if (health == 0)
        {
            if (TryGetComponent(out PlayerStateMachine playerStateMachine))
            {
                playerStateMachine.InvokeOnPlayerDead();
            }

            OnDie?.Invoke();
        }
    }

    public void InstantiateRagdoll(Transform ragdoll, WeaponSO currentWeapon)
    {
        Transform ragdollTransform = Instantiate(ragdoll, transform.position, transform.rotation);
        SetRagdollObjectWeapon(ragdollTransform, currentWeapon);

        MatchAllChildrenTransform(transform, ragdollTransform);
        ApplyExplosionToRagdoll(ragdollTransform, 500f, new Vector3(0, 0, 0), 10f);

        Destroy(gameObject);
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
}
