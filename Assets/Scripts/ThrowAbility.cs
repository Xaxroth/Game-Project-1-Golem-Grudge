using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ThrowAbility : MonoBehaviour
{

    [SerializeField] private Transform ShootPosition;
    [SerializeField] private GameObject ShootObject;
    [SerializeField] [Range(1, 50)] private float throwForce;
    [SerializeField] private float maxThrowForce = 50;
    [SerializeField] private float _multiplier = 0.01f;
    [SerializeField] private bool chargingThrow;
    [SerializeField] private Transform followObject;
    [SerializeField] private Transform lookObject;
    [SerializeField] private CinemachineFreeLook CinemachineFreeLookCamera;
    private bool throwingObject;

    void Start()
    {
        
    }

    public void RockThrow(InputAction.CallbackContext context)
    {
        if (gameObject.GetComponent<InputController>().beingControlled)
        {
            if (context.phase == InputActionPhase.Performed && !throwingObject)
            {
                throwForce = 0;
                chargingThrow = true;
            }
            else if (context.phase == InputActionPhase.Canceled && !throwingObject)
            {
                chargingThrow = false;
                StartCoroutine(RockThrowCoroutine());
            }
        }
    }

    private IEnumerator RockThrowCoroutine()
    {
        followObject = CinemachineFreeLookCamera.Follow;
        lookObject = CinemachineFreeLookCamera.LookAt;

        GameManager.actionHappening = true;

        throwingObject = true;

        GameObject heavyRock = Instantiate(ShootObject, ShootPosition.transform.position, ShootPosition.transform.rotation);

        CinemachineFreeLookCamera.Follow = heavyRock.gameObject.transform;
        CinemachineFreeLookCamera.LookAt = heavyRock.gameObject.transform;

        heavyRock.GetComponent<Rigidbody>().AddForce(ShootPosition.transform.up * throwForce, ForceMode.Impulse);
        //GameManager.NewPlayer();
        yield return new WaitForSeconds(5);

        GameManager.endTurn = true;
        GameManager.actionHappening = false;

        CinemachineFreeLookCamera.Follow = followObject;
        CinemachineFreeLookCamera.LookAt = lookObject;
        throwingObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (chargingThrow)
        {
            throwForce += _multiplier;
            throwForce = Mathf.Clamp(throwForce, 0, maxThrowForce);
        }
    }
}
