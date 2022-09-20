using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class InputController : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private int _health = 1;
    [SerializeField] [Range(0f, 10f)] private float _groundDrag = 6f;
    [SerializeField] [Range(0f, 10f)] private float _jumpStrength = 3f;
    [SerializeField] [Range(0f, 1f)] private float _movementSpeed = 0.75f;
    [SerializeField] [Range(5, 15)] private float _gravityModifier = 9.8f;

    [SerializeField] public Transform zoomFollowObject;
    [SerializeField] private Transform followObject;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private LayerMask layerMask;

    // Cosmetics
    [SerializeField] private CinemachineFreeLook _cineMachineCamera;
    [SerializeField] private ParticleSystem HoverParticles;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip hoveringSound;

    [Range(0f, 20f)] public float hoveringPower = 20f;
    [Range(0f, 20f)] public float maximumHoveringPower = 20f;

    [SerializeField] private bool _isDead = false;
    [SerializeField] private bool _hovering = false;

    public bool beingControlled;
    public bool canBeControlled;
    public bool onGround;

    public Vector2 moveValue;
    private Vector3 _jumpDirection = new Vector3(0, 5, 0);

    void Awake()
    {
        canBeControlled = true;
        Cursor.lockState = CursorLockMode.Locked;
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        playerAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {

        if (canBeControlled && beingControlled && !GameManager.actionHappening)
        {
            var moveVector = new Vector3(moveValue.x, 0, moveValue.y);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.rotation.eulerAngles.z);

            if (onGround)
            {
                playerRigidbody.AddForce((transform.right * moveVector.x + transform.forward * moveVector.z) * _movementSpeed, ForceMode.Impulse);
            }
            else
            {
                playerRigidbody.AddForce((transform.right * moveVector.x + transform.forward * moveVector.z) * _movementSpeed, ForceMode.Impulse);
            }
        }
    }

    public void Jump()
    {
        if (canBeControlled && beingControlled && !GameManager.actionHappening)
        {
            if (onGround)
            {
                playerRigidbody.AddForce(_jumpDirection * _jumpStrength, ForceMode.Impulse);
            }
        }
    }

    public void Hover(InputAction.CallbackContext context)
    {
        if (canBeControlled && beingControlled && !GameManager.actionHappening)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                playerAudioSource.clip = hoveringSound;
                playerAudioSource.volume = 3f;
                playerAudioSource.Play();
                HoverParticles.Play();
                _hovering = true;
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                playerAudioSource.Stop();
                HoverParticles.Stop();
                _hovering = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_health != 0)
        {
            _health -= damage;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else
        {
            Debug.Log("ACK!");
            _isDead = true;
            playerRigidbody.isKinematic = true;
            playerBody.SetActive(false);
            StartCoroutine(DeathCoroutine());
        }
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1);
    }

    void Update()
    {
        if (_hovering == true && hoveringPower > 0f && beingControlled && canBeControlled && !GameManager.actionHappening)
        {
            playerRigidbody.AddForce(_jumpDirection * 0.03f, ForceMode.Impulse);
            hoveringPower -= 0.03f;
        }
        else
        {

        }

        if (_hovering && hoveringPower < 0f && beingControlled && canBeControlled && !GameManager.actionHappening)
        {
            _hovering = false;
            hoveringPower = 0f;
        }

        if (playerRigidbody.velocity.y <= 0 && _hovering == false)
        {
            playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (_gravityModifier - 1f) * Time.deltaTime;
        }

        if (onGround == true)
        {
            playerRigidbody.drag = _groundDrag;
        }
        else
        {

        }
    }
}
