using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine, ISaveable
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
    [field: SerializeField] public Transform Ragdoll { get; private set; }

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
        PlayerAnimator.SetOverrideAnimation(Animator, CurrentWeapon.animatorOverride);

        WeaponLogic.transform.position = weapon.position;
        WeaponLogic.transform.rotation = weapon.rotation;
    }

    private void Health_OnTakeDamage(GameObject sender)
    {
        SwitchState(new PlayerImpactState(this));
    }

    private void Health_OnDie()
    {
        SwitchState(new PlayerDeadState(this));
    }

    public void SetDodgeTime(float dodgeTime)
    {
        PreviousDodgeTime = dodgeTime;
    }

    public object CaptureState()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["transform"] = new SerializableTransform(transform);
        data["scene"] = Loader.GetActiveSceneName();

        return data;
    }

    public async void RestoreState(object state)
    {
        Dictionary<string, object> data = (Dictionary<string, object>)state;

        ForceReciver.Reset();

        await Loader.LoadScene((string)data["scene"]);

        //Somethimes player is loading under ground. Change in future.
        CharacterController.transform.position = ((SerializableTransform)data["transform"]).ToTransform().Position + new Vector3(0, 6f, 0);
        CharacterController.enabled = false;
        CharacterController.transform.rotation = ((SerializableTransform)data["transform"]).ToTransform().Rotation;
        CharacterController.enabled = true;
    }
}
