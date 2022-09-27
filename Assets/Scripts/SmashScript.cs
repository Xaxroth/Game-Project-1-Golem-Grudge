using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SmashScript : MonoBehaviour
{
    //[SerializeField] [Range(1f, 50f)] private float _throwForce;
    //[SerializeField] [Range(1f, 50f)] private float _maxThrowForce = 50;
    //[SerializeField] [Range(0.01f, 1f)] private float _multiplier = 0.01f;

    [SerializeField] [Range(0, 100f)] private float punchForce = 100;
    [SerializeField] [Range(0, 10f)] private float punchRadius = 10;
    [SerializeField] [Range(0, 10)] private int punchDamage = 50;

    private bool _smashing;

    [SerializeField] private GameObject CinemachineZoomCamera;
    [SerializeField] private GameObject ExplosionPrefab;
    [SerializeField] private GameObject ShootObject;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip crowdCheerSound;

    [SerializeField] private Transform ShootPosition;
    [SerializeField] private Transform FollowObject;
    [SerializeField] private Transform LookObject;
    [SerializeField] private Transform AimTarget;

    [SerializeField] private CinemachineFreeLook CinemachineFreeLookCamera;

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

        yield return new WaitForSeconds(1.3f);

        GameManager.endTurn = true;
        GameManager.actionHappening = false;

        if (gameObject.GetComponent<InputController>()._isDead)
        {
            gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1);

        _smashing = false;
    }
}
