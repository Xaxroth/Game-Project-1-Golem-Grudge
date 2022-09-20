using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundcheckScript : MonoBehaviour
{
    [SerializeField] private InputController player;

    void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            player.onGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            player.onGround = false;
        }
    }
}
