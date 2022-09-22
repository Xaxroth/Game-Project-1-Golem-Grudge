using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class InputController : MonoBehaviour
{
    [Range(0, 100)] public int health = 100;
    
    [SerializeField] [Range(0f, 10f)] private float _groundDrag = 6f;
    [SerializeField] [Range(0f, 10f)] private float _jumpStrength = 3f;
    [SerializeField] [Range(0f, 1f)] private float _movementSpeed = 0.75f;
    [SerializeField] [Range(5, 15)] private float _gravityModifier = 9.8f;

    [SerializeField] private GameManager _gameManager;

    [SerializeField] public Transform zoomFollowObject;
    [SerializeField] private Transform followObject;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathExplosion;

    // Cosmetics
    [SerializeField] private CinemachineFreeLook _cineMachineCamera;
    [SerializeField] private ParticleSystem HoverParticles;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip hoveringSound;

    [Range(0f, 20f)] public float hoveringPower = 20f;
    [Range(0f, 20f)] public float maximumHoveringPower = 20f;

    [SerializeField] public bool _isDead = false;
    [SerializeField] private bool _hovering = false;
    public int playerNumber = 1;

    private PlayerInputAction myInputActions;

    public bool beingControlled;
    public bool canBeControlled;
    public bool onGround;

    public Vector2 moveValue;
    private Vector3 _jumpDirection = new Vector3(0, 5, 0);

    void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.allGolems.Add(this);

        myInputActions = new PlayerInputAction();
        myInputActions.Enable();

        canBeControlled = true;
        Cursor.lockState = CursorLockMode.Locked;
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        playerAudioSource = gameObject.GetComponent<AudioSource>();
        playerRigidbody.drag = _groundDrag;
    }

    public void MovementListener(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
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

    public void PlayerMovement()
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
        if (health > 0)
        {
            Debug.Log("ACK! " + damage);
            health -= damage;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        if (health <= 0 && !_isDead)
        {
            _gameManager.allGolems.Remove(this);

            switch (playerNumber)
            {
                case 1:
                    GameManager.amountOfRedGolemsLeft--;
                    _gameManager.redGolemList.Remove(this);
                    break;
                case 2:
                    GameManager.amountOfBlueGolemsLeft--;
                    _gameManager.blueGolemList.Remove(this);
                    break;
            }

            GameObject explosion = Instantiate(deathExplosion, transform.position, transform.rotation);
            playerAudioSource.PlayOneShot(deathSound);
            Debug.Log("ACK! I AM DEAD!!");
            
            _isDead = true;
            playerRigidbody.isKinematic = true;
            playerBody.SetActive(false);
            gameObject.GetComponent<InputController>().enabled = false;
        }
    }

    private void HoverCheck()
    {
        if (_hovering == true && hoveringPower > 0f && beingControlled && canBeControlled && !GameManager.actionHappening)
        {
            playerRigidbody.AddForce(_jumpDirection * 0.03f, ForceMode.Impulse);
            hoveringPower -= 0.03f;
        }

        if (_hovering && hoveringPower < 0f && beingControlled && canBeControlled && !GameManager.actionHappening)
        {
            _hovering = false;
            hoveringPower = 0f;
        }

        if (beingControlled == false)
        {
            _hovering = false;
        }
    }

    void Update()
    {
        HoverCheck();

        if (playerRigidbody.velocity.y <= 0 && _hovering == false)
        {
            playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (_gravityModifier - 1f) * Time.deltaTime;
        }
    }
}
