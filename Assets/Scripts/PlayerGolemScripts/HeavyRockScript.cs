using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRockScript : MonoBehaviour
{
    [Header("Projectile Properties")]
    [SerializeField] [Range(1, 25)] private float _throwForce;
    [SerializeField] [Range(1, 50)] private float _explosionRadius;
    [SerializeField] [Range(1, 500)] private float _explosionForce;
    [SerializeField] [Range(1, 150)] private float _explosionDamage;

    [Header("Logistics")]
    [SerializeField] private bool _detonationOccured;
    [SerializeField] private Rigidbody _RockRigidbody;
    [SerializeField] private InputController _PlayerController;

    [Header("Cosmetics")]
    [SerializeField] private GameObject _ExplosionPrefab;
    [SerializeField] private ParticleSystem _FireTrail;
    [SerializeField] private ParticleSystem _FireParticles;

    private void Awake()
    {
        _RockRigidbody = gameObject.GetComponentInChildren<Rigidbody>();
        StartCoroutine(SelfDestruct());
        _explosionDamage = 150f;
    }
    private void Update()
    {
        if (_RockRigidbody.velocity.y <= 0f)
        {
            _RockRigidbody.velocity += Vector3.up * Physics.gravity.y * (3f - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment") && !_detonationOccured || collision.gameObject.CompareTag("Player"))
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        GameObject explosion = Instantiate(_ExplosionPrefab, transform.position, _ExplosionPrefab.transform.rotation);

        _detonationOccured = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider near in colliders)

        {
            Rigidbody targetRigidbodies = near.GetComponent<Rigidbody>();

            if (targetRigidbodies != null && targetRigidbodies.gameObject.tag != "Projectile")
            {
                targetRigidbodies.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 20f, ForceMode.Impulse);
            }

        }

        var hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var hitCollider in hitColliders)
        {

            var targetHit = hitCollider.GetComponent<InputController>();

            if (targetHit)
            {

                var closestPoint = hitCollider.ClosestPoint(transform.position);

                var distance = Vector3.Distance(closestPoint, transform.position);

                var explosionDamage = Mathf.InverseLerp(_explosionRadius, 0, distance);

                targetHit.TakeDamage((int)(explosionDamage * 100));

            }

        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        _FireTrail.Stop();
        _FireParticles.Stop();

        Destroy(gameObject, 3f);
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);

        if (!_detonationOccured)
        {
            Explosion();
            Destroy(gameObject);
        }
    }
}
