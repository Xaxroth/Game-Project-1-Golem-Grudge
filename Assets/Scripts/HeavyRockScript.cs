using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRockScript : MonoBehaviour
{
    [SerializeField] private Rigidbody RockRigidbody;
    [SerializeField] [Range(1, 25)] private float throwForce;
    [SerializeField] [Range(1, 50)] private float explosionRadius;
    [SerializeField] [Range(1, 500)] private float explosionForce;
    [SerializeField] [Range(1, 50)] private float explosionDamage;
    [SerializeField] private bool detonationOccured;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private InputController playerController;

    [SerializeField] private ParticleSystem fireTrail;
    [SerializeField] private ParticleSystem fireParticles;


    private Quaternion rockRotation = Quaternion.Euler(1, 0, 0);

    void Start()
    {
        RockRigidbody = gameObject.GetComponentInChildren<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment") && !detonationOccured || collision.gameObject.CompareTag("Player"))
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
                
            detonationOccured = true;

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
                
            foreach (Collider near in colliders)
                
            {
                    Rigidbody targetRigidbodies = near.GetComponent<Rigidbody>();

                    if (targetRigidbodies != null && targetRigidbodies.gameObject.tag != "Projectile")
                    {
                        targetRigidbodies.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
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

                        
                    var damage = Mathf.InverseLerp(explosionRadius, 0, distance);

                    targetHit.TakeDamage((int)explosionDamage);

                }
                
            }

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            fireTrail.Stop();
            fireParticles.Stop();
            Destroy(gameObject, 3f);
        }
    }

    void Update()
    {
        if (RockRigidbody.velocity.y <= 0f)
        {
            RockRigidbody.velocity += Vector3.up * Physics.gravity.y * (3f - 1) * Time.deltaTime;
        }
    }
}
