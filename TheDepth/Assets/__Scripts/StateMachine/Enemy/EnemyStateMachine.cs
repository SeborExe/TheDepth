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
    [field: SerializeField] public float RotationSpeed { get; private set; }

    [field: Header("Attack")]
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int ChanceToRollAfterAttack { get; private set; }
    [field: SerializeField] public int ChanceToRollAfterTakeDamage { get; private set; }

    [field: Header("Detection Player")]
    [field: SerializeField] public LayerMask Detectionlayer { get; private set; }
    [field: SerializeField] public float PlayerDetectionRange { get; private set; }
    [field: SerializeField] public float MinDetectionAngle { get; private set; }
    [field: SerializeField] public float MaxDetectionAngle { get; private set; }
    public Health Player { get; private set; }

    [field: Header("Suspicious and Patroling")]
    [field: SerializeField] public PatrolPath PatrolPath { get; private set; }
    [field: SerializeField] public float PercentSpeedWhenBackToPosition { get; private set; }
    [field: SerializeField] public float TimeInSuspiciousState { get; private set; }
    [field: SerializeField] public float PercentSpeedInPatrolingState { get; private set; }
    [field: SerializeField] public float TimeInPatrolCheckPoint { get; private set; }
    public Vector3 startPosition { get; private set; }
    public Quaternion startRotation { get; private set; }

    [field: Header("Weapon")]
    [field: SerializeField] public WeaponDamage WeaponLogic { get; private set; }
    [field: SerializeField] public WeaponSO CurrentWeapon { get; private set; }
    [field: SerializeField] public Transform WeaponTransform { get; private set; }

    [field: Header("Damaged")]
    [field: SerializeField] public Transform Ragdoll { get; private set; }

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

        startPosition = transform.position;
        startRotation = transform.rotation;

        InitializeWeapon();
        
        if (PatrolPath != null)
        {
            SwitchState(new EnemyPatrolingState(this));
        }
        else
        {
            SwitchState(new EnemyIdleState(this));
        }
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += Health_OnTakeDamage;
        Health.OnDie += Health_OnDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= Health_OnTakeDamage;
        Health.OnDie -= Health_OnDie;
    }

    private void InitializeWeapon()
    {
        Transform weapon = Instantiate(CurrentWeapon.weaponPrefab.weaponGameObject, WeaponTransform).transform;
        WeaponLogic.Initialize(CurrentWeapon.weaponPrefab.weaponMesh);
        EnemyAnimator.SetOverrideAnimation(Animator, CurrentWeapon.animatorOverride);

        WeaponLogic.transform.position = weapon.position;
        WeaponLogic.transform.rotation = weapon.rotation;
    }

    private void Health_OnTakeDamage(GameObject sender)
    {
        SwitchState(new EnemyImpactState(this, sender));
    }

    private void Health_OnDie()
    {
        SwitchState(new EnemyDeadState(this));
    }

    public void ChangePlayerDetection(Health player)
    {
        if (player != null)
        {
            Player = player;
        }
        else
        {
            Player = null;
            SwitchState(new EnemyIdleState(this));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRange);
    }
}
