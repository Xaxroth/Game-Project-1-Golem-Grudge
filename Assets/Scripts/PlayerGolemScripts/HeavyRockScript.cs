using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRockScript : MonoBehaviour
{
    [Header("Projectile Properties")]
    [SerializeField] [Range(1, 25)] private float throwForce;
    [SerializeField] [Range(1, 50)] private float explosionRadius;
    [SerializeField] [Range(1, 500)] private float explosionForce;
    [SerializeField] [Range(1, 50)] private float explosionDamage;

    [Header("Logistics")]
    [SerializeField] private bool detonationOccured;
    [SerializeField] private Rigidbody RockRigidbody;
    [SerializeField] private InputController playerController;

    [Header("Cosmetics")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private ParticleSystem fireTrail;
    [SerializeField] private ParticleSystem fireParticles;

    private void Awake()
    {
        RockRigidbody = gameObject.GetComponentInChildren<Rigidbody>();
        StartCoroutine(SelfDestruct());
        explosionDamage = 150f;
    }
    private void Update()
    {
        if (RockRigidbody.velocity.y <= 0f)
        {
            RockRigidbody.velocity += Vector3.up * Physics.gravity.y * (3f - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment") && !detonationOccured || collision.gameObject.CompareTag("Player"))
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);

        detonationOccured = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider near in colliders)

        {
            Rigidbody targetRigidbodies = near.GetComponent<Rigidbody>();

            if (targetRigidbodies != null && targetRigidbodies.gameObject.tag != "Projectile")
            {
                targetRigidbodies.AddExplosionForce(explosionForce, transform.position, explosionRadius, 20f, ForceMode.Impulse);
            }

        }

        var hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var hitCollider in hitColliders)
        {

            var targetHit = hitCollider.GetComponent<InputController>();

            if (targetHit)
            {

                var closestPoint = hitCollider.ClosestPoint(transform.position);

                var distance = Vector3.Distance(closestPoint, transform.position);

                var explosionDamage = Mathf.InverseLerp(explosionRadius, 0, distance);

                targetHit.TakeDamage((int)(explosionDamage * 100));

            }

        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        fireTrail.Stop();
        fireParticles.Stop();

        Destroy(gameObject, 3f);
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);

        if (!detonationOccured)
        {
            Explosion();
            Destroy(gameObject);
        }
    }
}
