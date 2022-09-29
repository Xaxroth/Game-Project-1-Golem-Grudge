using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SmashScript : MonoBehaviour
{
    [Header("Ability Values")]
    [SerializeField] [Range(0, 100f)] private float punchForce = 100;
    [SerializeField] [Range(0, 10f)] private float punchRadius = 4;
    [SerializeField] [Range(0, 10)] private int punchDamage = 50;

    [Header("Cosmetics")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioSource playerAudioSource;

    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip crowdCheerSound;

    [SerializeField] private GameObject ExplosionPrefab;

    [Header("Camera")]
    [SerializeField] private Transform ShootPosition;
    [SerializeField] private Transform FollowObject;
    [SerializeField] private Transform LookObject;
    [SerializeField] private Transform AimTarget;

    [SerializeField] private GameObject ShootObject;
    [SerializeField] private GameObject CinemachineZoomCamera;
    
    [SerializeField] private CinemachineFreeLook CinemachineFreeLookCamera;

    private bool _smashing;

    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Smash(InputAction.CallbackContext context)
    {
        if (gameObject.GetComponent<InputController>().beingControlled && !GameManager.actionHappening && !_smashing)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                playerAnimator.SetBool("Punch", true);
                StartCoroutine(SmashCoroutine());
            }
        }
    }

    private IEnumerator SmashCoroutine()
    {
        playerAudioSource.PlayOneShot(punchSound);

        GameManager.actionHappening = true;
        _smashing = true;

        yield return new WaitForSeconds(1.7f);

        playerAnimator.SetBool("Punch", false);

        Collider[] colliders = Physics.OverlapSphere(transform.position, punchRadius);

        foreach (Collider near in colliders)

        {
            Rigidbody targetRigidbodies = near.GetComponent<Rigidbody>();
            InputController targetInputControllers = near.GetComponent<InputController>();

            if (targetRigidbodies != null && targetRigidbodies.gameObject.tag != "Projectile")
            {
                if (targetRigidbodies != gameObject.GetComponent<Rigidbody>())
                {
                    targetInputControllers.TakeDamage(punchDamage);
                    targetInputControllers.punched = true;

                    targetRigidbodies.AddForce(gameObject.transform.forward * 20, ForceMode.Impulse);
                    targetRigidbodies.AddForce(gameObject.transform.up * 20, ForceMode.Impulse);

                    GameObject explosion = Instantiate(ExplosionPrefab, targetRigidbodies.gameObject.transform.position, transform.rotation);

                    yield return new WaitForSeconds(0.5f);

                    playerAudioSource.PlayOneShot(crowdCheerSound, 0.5f);

                    yield return new WaitForSeconds(1.5f);

                    targetInputControllers.punched = false;

                }
            }

        }

        GameManager.endTurn = true;

        yield return new WaitForSeconds(2f);

        GameManager.actionHappening = false;

        if (gameObject.GetComponent<InputController>()._isDead)
        {
            gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1);

        _smashing = false;
    }
}
