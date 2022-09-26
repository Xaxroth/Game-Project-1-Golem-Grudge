using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SmashScript : MonoBehaviour
{
    [SerializeField] [Range(1f, 50f)] private float _throwForce;
    [SerializeField] [Range(1f, 50f)] private float _maxThrowForce = 50;
    [SerializeField] [Range(0.01f, 1f)] private float _multiplier = 0.01f;

    private bool _smashing;

    [SerializeField] private GameObject CinemachineZoomCamera;
    [SerializeField] private GameObject ExplosionPrefab;
    [SerializeField] private GameObject ShootObject;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private Transform ShootPosition;
    [SerializeField] private Transform FollowObject;
    [SerializeField] private Transform LookObject;
    [SerializeField] private Transform AimTarget;

    [SerializeField] private CinemachineFreeLook CinemachineFreeLookCamera;

    void Start()
    {
        AimTarget.transform.position = gameObject.GetComponent<InputController>().zoomFollowObject.transform.position;
        playerAnimator = gameObject.GetComponent<Animator>();
        //CinemachineZoomCamera = GameObject.FindGameObjectWithTag("ZoomCamera");
        //CinemachineFreeLookCamera = CinemachineZoomCamera.GetComponent<CinemachineFreeLook>();
    }

    public void Smash(InputAction.CallbackContext context)
    {
        if (gameObject.GetComponent<InputController>().beingControlled)
        {
            if (context.phase == InputActionPhase.Performed && !_smashing)
            {
                Debug.Log("ACKKKK");
                playerAnimator.SetBool("Punch", true);
                StartCoroutine(SmashCoroutine());
            }
            //else if (context.phase == InputActionPhase.Canceled)
            //{
            //    _chargingSmash = false;
            //    playerAnimator.SetBool("Punch", false);
            //    StartCoroutine(SmashCoroutine());
            //}
        }
    }

    //public void ZoomIn(InputAction.CallbackContext context)
    //{
    //    if (context.phase == InputActionPhase.Performed && _chargingSmash == true)
    //    {
    //        CinemachineZoomCamera.SetActive(true);
    //        CinemachineZoomCamera.GetComponent<CinemachineFreeLook>().Follow = gameObject.GetComponent<InputController>().zoomFollowObject;
    //        CinemachineZoomCamera.GetComponent<CinemachineFreeLook>().LookAt = gameObject.GetComponent<InputController>().zoomFollowObject;
    //    }
    //    else if (context.phase == InputActionPhase.Canceled && _chargingSmash == true)
    //    {
    //        CinemachineZoomCamera.SetActive(false);
    //    }
    //}

    private IEnumerator SmashCoroutine()
    {
        FollowObject = CinemachineFreeLookCamera.Follow;
        LookObject = CinemachineFreeLookCamera.LookAt;

        GameManager.actionHappening = true;
        _smashing = true;

        //GameObject heavyRock = Instantiate(ShootObject, ShootPosition.transform.position, ShootPosition.transform.rotation);

        //CinemachineFreeLookCamera.Follow = heavyRock.gameObject.transform;
        //CinemachineFreeLookCamera.LookAt = heavyRock.gameObject.transform;

        //heavyRock.GetComponent<Rigidbody>().AddForce(ShootPosition.transform.forward * _throwForce, ForceMode.Impulse);
        //heavyRock.GetComponent<Rigidbody>().AddForce(ShootPosition.transform.up / 5 * _throwForce, ForceMode.Impulse);

        yield return new WaitForSeconds(1.6f);
        playerAnimator.SetBool("Punch", false);

        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(3.4f);

        GameManager.endTurn = true;
        GameManager.actionHappening = false;
        _smashing = false;

        if (gameObject.GetComponent<InputController>()._isDead)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        CinemachineZoomCamera.transform.position = AimTarget.transform.position;
    }

    void Update()
    {

        //if (_chargingSmash)
        //{
        //    _throwForce += _multiplier;
        //    _throwForce = Mathf.Clamp(_throwForce, 0, _maxThrowForce);
        //}

        ShootPosition.transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (GameManager.actionHappening)
        {
            CinemachineZoomCamera.SetActive(false);
        }
    }
}
