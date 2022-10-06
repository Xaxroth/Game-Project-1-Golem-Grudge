using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundcheckScript : MonoBehaviour
{
    [SerializeField] private InputController _Player;

    private void Awake()
    {
        _Player = GetComponentInParent<InputController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            _Player.onGround = true;
            _Player.gameObject.GetComponent<Rigidbody>().drag = 6;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            _Player.onGround = false;
        }
    }
}
