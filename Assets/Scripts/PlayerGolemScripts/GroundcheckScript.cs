using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundcheckScript : MonoBehaviour
{
    [SerializeField] private InputController player;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            player.onGround = true;
            player.gameObject.GetComponent<Rigidbody>().drag = 6;
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
