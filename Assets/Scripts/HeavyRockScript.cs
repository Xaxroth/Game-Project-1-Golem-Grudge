using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRockScript : MonoBehaviour
{
    [SerializeField] private Rigidbody RockRigidbody;
    [SerializeField] [Range(1, 25)] private float throwForce;
    [SerializeField] [Range(1, 50)] private float explosionRadius;
    [SerializeField] [Range(1, 50)] private float explosionForce;
    [SerializeField] [Range(1, 50)] private float explosionDamage;
    [SerializeField] private bool detonationOccured;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private InputController playerController;
    private Quaternion rockRotation = Quaternion.Euler(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        RockRigidbody = gameObject.GetComponent<Rigidbody>();
        //RockRigidbody.AddForce(transform.up * throwForce / 5, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment") && !detonationOccured)
        {
            // KABOOM and stuff
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
                
            detonationOccured = true;

            //playerController.GetComponent<InputController>().TakeDamage((int)explosionDamage);
                
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

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RockRigidbody.velocity.y <= 0f)
        {
            //RockRigidbody.velocity.y = new Vector3(RockRigidbody.velocity.x, Physics.gravity.magnitude, RockRigidbody.velocity.z);
            RockRigidbody.velocity += Vector3.up * Physics.gravity.y * (3f - 1) * Time.deltaTime;
        }
    }
}
