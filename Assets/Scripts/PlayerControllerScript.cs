using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] [Range(0f, 100f)] private float playerMovementSpeed;
    private CharacterController _playerCharacterController;
    private Vector2 _moveValue;

    private void Awake()
    {
        _playerCharacterController = gameObject.GetComponent<CharacterController>();
    }
    void Start()
    {
        
    }

    public void Move()
    {

    }

    //private void FixedUpdate()
    //{
    //    var moveVector = new Vector3(_moveValue.x, 0, _moveValue.y);
    //    _playerCharacterController.Move(moveVector * playerMovementSpeed);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
