using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerAnimator PlayerAnimator { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public ForceReciver ForceReciver { get; private set; }
    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public float TimeWhilePlayerIsAbleToRotate { get; private set; }
    [field: SerializeField] public WeaponSO CurrentWeapon { get; private set; }
    [field: SerializeField] public Transform WeaponTransform { get; private set; }

    private void Awake()
    {
        PlayerAnimator = GetComponentInChildren<PlayerAnimator>();
        InputHandler = GetComponent<InputHandler>();
        CharacterController = GetComponent<CharacterController>();
        ForceReciver = GetComponent<ForceReciver>();
    }

    private void Start()
    {
        InitializeWeapon();
        SwitchState(new PlayerMoveState(this));
    }

    private void InitializeWeapon()
    {
        Instantiate(CurrentWeapon.weaponPrefab.weaponGameObject, WeaponTransform);
        WeaponDamage.Initialize(CurrentWeapon.weaponPrefab.weaponMesh);
        PlayerAnimator.SetOverrideAnimation(CurrentWeapon.animatorOverride);
    }
}
