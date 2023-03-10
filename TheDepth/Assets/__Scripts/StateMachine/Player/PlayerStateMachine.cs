using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerAnimator PlayerAnimator { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public ForceReciver ForceReciver { get; private set; }
    public Animator Animator { get; private set; }
    public Health Health { get; private set; }
    [field: SerializeField] public WeaponDamage WeaponLogic { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField, HideInInspector] public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;
    [field: SerializeField] public float DodgeCoolDown { get; private set; }
    [field: SerializeField] public float TimeWhilePlayerIsAbleToRotate { get; private set; }
    [field: SerializeField] public WeaponSO CurrentWeapon { get; private set; }
    [field: SerializeField] public Transform WeaponTransform { get; private set; }

    private void Awake()
    {
        PlayerAnimator = GetComponentInChildren<PlayerAnimator>();
        InputHandler = GetComponent<InputHandler>();
        CharacterController = GetComponent<CharacterController>();
        ForceReciver = GetComponent<ForceReciver>();
        Animator = GetComponentInChildren<Animator>();
        Health = GetComponent<Health>();
    }

    private void Start()
    {
        InitializeWeapon();
        SwitchState(new PlayerMoveState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += Health_OnTakeDamage;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= Health_OnTakeDamage;
    }

    private void InitializeWeapon()
    {
        Transform weapon = Instantiate(CurrentWeapon.weaponPrefab.weaponGameObject, WeaponTransform).transform;
        WeaponLogic.Initialize(CurrentWeapon.weaponPrefab.weaponMesh);
        PlayerAnimator.SetOverrideAnimation(Animator, CurrentWeapon.animatorOverride);

        WeaponLogic.transform.position = weapon.position;
        WeaponLogic.transform.rotation = weapon.rotation;
    }

    private void Health_OnTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    public void SetDodgeTime(float dodgeTime)
    {
        PreviousDodgeTime = dodgeTime;
    }
}
