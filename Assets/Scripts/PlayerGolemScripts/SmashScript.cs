using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SmashScript : MonoBehaviour
{
    [Header("Ability Values")]
    [SerializeField] [Range(0, 100f)] private float _punchForce = 100;
    [SerializeField] [Range(0, 10f)] private float _punchRadius = 4;
    [SerializeField] [Range(0, 10)] private int _punchDamage = 50;

    [Header("Cosmetics")]
    [SerializeField] private Animator _PlayerAnimator;
    [SerializeField] private AudioSource _PlayerAudioSource;

    [SerializeField] private AudioClip _PunchSound;
    [SerializeField] private AudioClip _CrowdCheerSound;

    [SerializeField] private GameObject _ExplosionPrefab;

    [SerializeField] private GameObject _CinemachineZoomCamera;
    
    [SerializeField] private CinemachineFreeLook _CinemachineFreeLookCamera;

    [SerializeField] private bool _smashing;

    void Start()
    {
        _PlayerAnimator = gameObject.GetComponent<Animator>();
        _PlayerAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Smash(InputAction.CallbackContext context)
    {
        if (gameObject.GetComponent<InputController>().beingControlled && !GameManager.actionHappening && !_smashing)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _PlayerAnimator.SetBool("Punch", true);
                StartCoroutine(SmashCoroutine());
            }
        }
    }

    private IEnumerator SmashCoroutine()
    {
        _PlayerAudioSource.PlayOneShot(_PunchSound);

        GameManager.actionHappening = true;
        _smashing = true;

        yield return new WaitForSeconds(1.7f);

        _PlayerAnimator.SetBool("Punch", false);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _punchRadius);

        foreach (Collider near in colliders)

        {
            Rigidbody targetRigidbodies = near.GetComponent<Rigidbody>();
            InputController targetInputControllers = near.GetComponent<InputController>();

            if (targetRigidbodies != null && targetRigidbodies.gameObject.tag != "Projectile")
            {
                if (targetRigidbodies != gameObject.GetComponent<Rigidbody>())
                {
                    targetInputControllers.TakeDamage(_punchDamage);
                    targetInputControllers.punched = true;

                    targetRigidbodies.AddForce(gameObject.transform.forward * 20, ForceMode.Impulse);
                    targetRigidbodies.AddForce(gameObject.transform.up * 20, ForceMode.Impulse);

                    GameObject explosion = Instantiate(_ExplosionPrefab, targetRigidbodies.gameObject.transform.position, transform.rotation);

                    yield return new WaitForSeconds(0.5f);

                    _PlayerAudioSource.PlayOneShot(_CrowdCheerSound, 0.5f);

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
