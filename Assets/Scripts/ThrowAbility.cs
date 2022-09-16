using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowAbility : MonoBehaviour
{

    [SerializeField] private Transform ShootPosition;
    [SerializeField] private GameObject ShootObject;
    [SerializeField] [Range(1, 25)] private float throwForce;
    private bool throwingObject;

    void Start()
    {
        
    }

    public void RockThrow()
    {
        if (!throwingObject)
        {
            StartCoroutine(RockThrowCoroutine());
        }

    }

    private IEnumerator RockThrowCoroutine()
    {
        throwingObject = true;
        GameObject heavyRock = Instantiate(ShootObject, ShootPosition.transform.position, ShootPosition.transform.rotation);

        heavyRock.GetComponent<Rigidbody>().AddForce(ShootPosition.transform.up * throwForce, ForceMode.Impulse);
        yield return new WaitForSeconds(1);
        throwingObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (RockThrowCoroutine is running)
    }
}
