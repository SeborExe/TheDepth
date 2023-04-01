using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, ISaveable
{
    public event Action<GameObject, bool> OnTakeDamage;
    public event Action OnDie;

    public bool IsDead => health.value == 0;

    private AnimatorController animatorController;
    private BaseStats baseStats;

    private LazyValue<float> health;
    private LazyValue<float> maxHealth;

    private Vector3 attackPosition;
    private GameObject ragdoll;

    private float healthPercentRegenerate = 40f;

    private void Awake()
    {
        animatorController = GetComponentInChildren<AnimatorController>();
        baseStats = GetComponent<BaseStats>();

        health = new LazyValue<float>(GetInitialHealth);
        maxHealth = new LazyValue<float>(GetInitialHealth);
    }

    private void Start()
    {
        health.ForceInit();
    }
     
    private float GetInitialHealth()
    {
        return GetMaxHealth();
    }

    public float GetMaxHealth()
    {
        return baseStats.GetStat(Stat.Health);
    }

    public void HealthOnLevelUp()
    {
        maxHealth.value = GetMaxHealth();

        float healthToRegenerate = GetMaxHealth() * (healthPercentRegenerate / 100f);
        health.value += healthToRegenerate;
    }

    public void Dealdamage(float damage, GameObject sender, Vector3 attackPosition, bool hasImpact)
    {
        if (health.value <= 0) { return; }
        if (animatorController.IsImmune) { return; }
        if (gameObject.tag == sender.tag) { return; }

        health.value = Mathf.Max(health.value - damage, 0);
        OnTakeDamage?.Invoke(sender, hasImpact);

        this.attackPosition = attackPosition;

        if (health.value == 0)
        {
            OnDie?.Invoke();
            AwardExperiene(sender);
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

    private void AwardExperiene(GameObject sender)
    {
        if (sender.TryGetComponent(out Player player))
        {
            player.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }
    }

    public float GetHealth()
    {
        return health.value;
    }

    public float GetMaxHealthValue()
    {
        return maxHealth.value;
    }

    public object CaptureState()
    {
        return health.value;
    }

    public void RestoreState(object state)
    {
        health.value = (float)state;
        if (health.value > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (ragdoll != null)
            {
                Destroy(ragdoll);
            }
        }
        else
        {
            OnDie?.Invoke();
            Destroy(ragdoll);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
