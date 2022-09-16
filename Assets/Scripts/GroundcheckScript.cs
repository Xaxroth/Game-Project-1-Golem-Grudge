using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundcheckScript : MonoBehaviour
{
    [SerializeField] private InputController player;
    // Start is called before the first frame update
    void Start()
    {
        //player = gameObject.transform.parent.GetComponent<PlayerControllerScript>();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
