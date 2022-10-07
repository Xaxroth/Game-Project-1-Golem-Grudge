using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("Logistics")]

    [SerializeField] private GameManager _GameManager;

    [SerializeField] private PlayerInputAction _MyInputActions;

    public int playerNumber = 1;

    [Header("Player Attributes")]

    [Range(0, 100)] public int health = 100;
    
    [SerializeField] [Range(0f, 10f)] private float _groundDrag = 1f;
    [SerializeField] [Range(0f, 10f)] private float _jumpStrength = 10f;
    [SerializeField] [Range(0f, 1f)] private float _movementSpeed = 0.75f;
    [SerializeField] [Range(5, 15)] private float _gravityModifier = 9.8f;

    [Range(0f, 20f)] public float hoveringPower = 20f;
    [Range(0f, 20f)] public float maximumHoveringPower = 20f;

    [Header("Camera")]

    [SerializeField] private Transform _FollowObject;
    [SerializeField] private CinemachineFreeLook _CinemachineCamera;

    [Header("Components")]

    public Transform zoomFollowObject;

    [SerializeField] private Rigidbody _PlayerRigidbody;
    [SerializeField] private GameObject _PlayerBody;


    [Header("Cosmetics")]

    [SerializeField] private ParticleSystem _HoverParticles;
    [SerializeField] private GameObject _DeathExplosion;

    [SerializeField] private AudioSource _PlayerAudioSource;
    [SerializeField] private AudioClip _HoveringSound;
    [SerializeField] private AudioClip _DeathSound;

    [Header("Conditions")]

    [SerializeField] private bool _hovering = false;

    public bool punched = false;
    public bool beingControlled;
    public bool canBeControlled;
    public bool onGround;
    public bool canMove;
    public bool _isDead = false;

    public float _distanceMoved;
    public Vector2 moveValue;
    public Vector3 _startPosition;
    private Vector3 _jumpDirection = new Vector3(0, 5, 0);

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _GameManager = FindObjectOfType<GameManager>();
        _GameManager.AllGolems.Add(this);

        _MyInputActions = new PlayerInputAction();
        _MyInputActions.Enable();

        _PlayerRigidbody = gameObject.GetComponent<Rigidbody>();
        _PlayerAudioSource = gameObject.GetComponent<AudioSource>();

        _CinemachineCamera = GameObject.FindGameObjectWithTag("CinemachineMainCamera").GetComponent<CinemachineFreeLook>();

        _PlayerRigidbody.drag = _groundDrag;
        _startPosition = transform.position;
        
        canBeControlled = true;
        canMove = true;
        _hovering = false;

        _HoverParticles.Stop();
    }

    void Update()
    {
        Conditions();
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
                _PlayerRigidbody.AddForce(_jumpDirection * _jumpStrength, ForceMode.Impulse);
            }
        }
    }

    public void PlayerMovement()
    {
        if (canBeControlled && beingControlled && !GameManager.actionHappening)
        {
            var moveVector = new Vector3(moveValue.x, 0, moveValue.y);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.rotation.eulerAngles.z);

            if (canMove)
            {
                if (onGround)
                {
                    _PlayerRigidbody.AddForce((transform.right * moveVector.x + transform.forward * moveVector.z) * _movementSpeed, ForceMode.Impulse);
                }
                else
                {
                    _PlayerRigidbody.AddForce((transform.right * moveVector.x + transform.forward * moveVector.z) * _movementSpeed, ForceMode.Impulse);
                }
            }

            if (moveValue != new Vector2(0, 0) && beingControlled)
            {
                _distanceMoved += Vector3.Distance(transform.position, _startPosition);
            }
        }
    }

    public void Hover(InputAction.CallbackContext context)
    {
        if (canBeControlled && beingControlled && !GameManager.actionHappening)
        {   
            if (context.phase == InputActionPhase.Performed)
            {
                _HoverParticles.Play();

                _hovering = true;
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                _HoverParticles.Stop();

                _hovering = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        if (health <= 0 && !_isDead)
        {
            _GameManager.AllGolems.Remove(this);

            switch (playerNumber)
            {
                case 1:
                    _GameManager.RedGolemList.Remove(this);
                    break;
                case 2:
                    _GameManager.BlueGolemList.Remove(this);
                    break;
                case 3:
                    _GameManager.GreenGolemList.Remove(this);
                    break;
                case 4:
                    _GameManager.PurpleGolemList.Remove(this);
                    break;
            }

            GameObject explosion = Instantiate(_DeathExplosion, transform.position, transform.rotation);

            _PlayerAudioSource.PlayOneShot(_DeathSound);

            _isDead = true;
            _PlayerRigidbody.isKinematic = true;
            _PlayerBody.SetActive(false);
            gameObject.GetComponent<InputController>().enabled = false;
        }
    }

    private void Conditions()
    {

        if (_distanceMoved >= 10000)
        {
            canMove = false;
        }

        if (_PlayerRigidbody.velocity.y <= 0 && _hovering == false)
        {

            _PlayerRigidbody.velocity += Vector3.up * Physics.gravity.y * (_gravityModifier) * Time.deltaTime;
        }

        if (_hovering == true && hoveringPower > 0f && beingControlled && canBeControlled && !GameManager.actionHappening)
        {
            _PlayerRigidbody.AddForce(_jumpDirection * 0.03f, ForceMode.Impulse);
            hoveringPower -= 0.03f;
        }

        if (_hovering && hoveringPower < 0f && beingControlled && canBeControlled && !GameManager.actionHappening)
        {
            _hovering = false;
            hoveringPower = 0f;
        }

        if (GameManager.actionHappening)
        {
            _hovering = false;
        }

        if (beingControlled == false)
        {
            _hovering = false;
        }

        if (punched)
        {
            _PlayerRigidbody.drag = 0;

        }
        else
        {
            _PlayerRigidbody.drag = 6;
        }
    }
}
