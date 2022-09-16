using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class InputController : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private int health = 100;

    private CharacterController playerCharacterController;
    private Rigidbody playerRigidbody;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private CinemachineFreeLook _cineMachineCamera;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private Transform followObject;

    private Vector3 direction;
    private Vector3 jumpDirection = new Vector3(0, 5, 0);
    private Vector3 fallDirection = new Vector3(0, -5, 0);

    public bool onGround;
    [SerializeField] [Range(5, 15)] private float gravityModifier;
    private const float playerHeight = 2;
    private float verticalMovement;
    private float horizontalMovement;

    [SerializeField] [Range(0f, 10f)] private float groundDrag = 6f;
    [SerializeField] [Range(0f, 2f)] private float airDrag = 2f;
    [SerializeField] [Range(0f, 10f)] private float jumpStrength;
    [SerializeField] [Range(0f, 1f)] private float movementSpeed;

    [SerializeField] private LayerMask layerMask;

    public Vector2 _moveValue;
    // Start is called before the first frame update
    void Awake()
    {
        //playerCharacterController = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        //Debug.Log("Moving");
        _moveValue = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        var moveVector = new Vector3(_moveValue.x, 0, _moveValue.y);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.rotation.eulerAngles.z);

        if (onGround)
        {
            playerRigidbody.AddForce((transform.right * moveVector.x + transform.forward * moveVector.z) * movementSpeed, ForceMode.Impulse);
        }
        else
        {
            playerRigidbody.AddForce((transform.right * moveVector.x + transform.forward * moveVector.z) * movementSpeed, ForceMode.Impulse);
        }
        
    }

    public void Jump()
    {
        if (onGround)
        {
            playerRigidbody.AddForce(jumpDirection * jumpStrength, ForceMode.Impulse);
        }
    }

    public void TakeDamage(int damage)
    {
        if (health != 0)
        {
            health -= damage;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //onGround = Physics.Raycast(playerRigidbody.gameObject.transform.position, Vector3.down, playerHeight / 2 + 0.2f);

        if (playerRigidbody.velocity.y <= 0)
        {
            playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (gravityModifier - 1f) * Time.deltaTime;
        }

        if (onGround == true)
        {
            playerRigidbody.drag = groundDrag;
        }
        else
        {
            //playerRigidbody.drag = airDrag;
            //playerRigidbody.AddForce(fallDirection * 0.015f, ForceMode.Impulse);
        }
    }
}
