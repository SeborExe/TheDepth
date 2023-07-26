using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rigidbody;
    private CinemachineImpulseSource source;
    private CapsuleCollider capsuleCollider;
    //private List<Collider> alreadyColliderWith = new List<Collider>();
    private bool dealedDamage = false;

    private Collider myCollider;
    private float damage;
    private float knockback;
    private bool hasImpact;
    private float force;

    [field: SerializeField] public ProjectileSO ProjectileSO { get; private set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody.centerOfMass = transform.position;
    }

    private void OnEnable()
    {
        //alreadyColliderWith.Clear();
    }

    public void SetUpAndFire(Collider myCollider, float damage, float knockback, bool hasImpact, float force)
    {
        this.myCollider = myCollider;
        this.damage = damage;
        this.knockback = knockback;
        this.hasImpact = hasImpact;
        this.force = force;

        rigidbody.AddForce(transform.forward * (70 * Random.Range(1.1f, 1.7f) * force), ForceMode.Impulse);
        source = GetComponent<CinemachineImpulseSource>();

        source.GenerateImpulse(Camera.main.transform.forward);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player")
        {
            if (collision.collider == myCollider) { return; }

            //Make Collider smaller 
            capsuleCollider.height = 0.1f;
            capsuleCollider.center = new Vector3(0, 0.03f, -0.06f);

            //dealedDamage = true;
            rigidbody.isKinematic = true;
            StartCoroutine(Countdown());
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name != "Player")
        {
            if (collision == myCollider) { return; }

            if (collision.gameObject.TryGetComponent(out Health health) && !dealedDamage)
            {
                health.Dealdamage(damage, myCollider.gameObject, collision.ClosestPoint(transform.position), hasImpact);
            }

            if (collision.gameObject.TryGetComponent(out ForceReciver forceReciver))
            {
                forceReciver.AddForce((collision.transform.position - myCollider.transform.position).normalized * knockback);
            }

            dealedDamage = true;
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
