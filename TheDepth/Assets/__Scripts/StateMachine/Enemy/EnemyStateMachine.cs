using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    public Animator Animator { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public ForceReciver ForceReciver { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Health Health { get; private set; }
    public EnemyAnimator EnemyAnimator { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }

    [field: Header("Attack")]
    [field: SerializeField] public float AttackRange { get; private set; }

    [field: Header("Detection Player")]
    [field: SerializeField] public LayerMask Detectionlayer { get; private set; }
    [field: SerializeField] public float PlayerDetectionRange { get; private set; }
    [field: SerializeField] public float MinDetectionAngle { get; private set; }
    [field: SerializeField] public float MaxDetectionAngle { get; private set; }
    public GameObject Player { get; private set; }

    [field: Header("Weapon")]
    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
    [field: SerializeField] public WeaponSO CurrentWeapon { get; private set; }
    [field: SerializeField] public Transform WeaponTransform { get; private set; }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        CharacterController = GetComponent<CharacterController>();
        ForceReciver = GetComponent<ForceReciver>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Health = GetComponent<Health>();
        EnemyAnimator = GetComponentInChildren<EnemyAnimator>();
    }

    private void Start()
    {
        NavMeshAgent.updatePosition = false;
        NavMeshAgent.updateRotation = false;

        InitializeWeapon();
        SwitchState(new EnemyIdleState(this));
    }

    private void InitializeWeapon()
    {
        Instantiate(CurrentWeapon.weaponPrefab.weaponGameObject, WeaponTransform);
        WeaponDamage.Initialize(CurrentWeapon.weaponPrefab.weaponMesh);
        EnemyAnimator.SetOverrideAnimation(Animator, CurrentWeapon.animatorOverride);
    }

    public void ChangePlayerDetection(GameObject player)
    {
        if (player != null)
        {
            Player = player;
            Debug.Log("Player Detected");
        }
        else
        {
            Player = null;
            Debug.Log("I don't know where player is...");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRange);
    }
}
