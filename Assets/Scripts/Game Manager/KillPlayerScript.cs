using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<InputController>().TakeDamage(other.gameObject.GetComponent<InputController>().health);
        }
    }
}
