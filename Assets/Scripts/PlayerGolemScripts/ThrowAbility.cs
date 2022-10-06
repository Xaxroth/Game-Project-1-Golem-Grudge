using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ThrowAbility : MonoBehaviour
{
    [Header("Ability Values")]

    [SerializeField] [Range(1f, 50f)] private float _throwForce;
    [SerializeField] [Range(1f, 50f)] private float _maxThrowForce = 50;
    [SerializeField] [Range(0.01f, 1f)] private float _multiplier = 0.01f;

    [Header("Camera")]

    [SerializeField] private GameObject _CinemachineZoomCamera;
    [SerializeField] private GameObject _ShootObject;

    [SerializeField] private Transform _ShootPosition;
    [SerializeField] private Transform _FollowObject;
    [SerializeField] private Transform _LookObject;
    [SerializeField] private Transform _AimTarget;

    [SerializeField] private CinemachineFreeLook _CinemachineFreeLookCamera;

    [SerializeField] private bool _throwingObject;
    [SerializeField] private bool _chargingThrow;

    private void Awake()
    {
        _AimTarget.transform.position = gameObject.GetComponent<InputController>().zoomFollowObject.transform.position;
        _CinemachineFreeLookCamera = GameObject.FindGameObjectWithTag("CinemachineMainCamera").GetComponent<CinemachineFreeLook>();
    }

    private void RockThrow(InputAction.CallbackContext context)
    {
        if (gameObject.GetComponent<InputController>().beingControlled)
        {
            if (context.phase == InputActionPhase.Performed && !_throwingObject && !GameManager.actionHappening)
            {
                _throwForce = 0;
                _chargingThrow = true;
            }
            else if (context.phase == InputActionPhase.Canceled && !_throwingObject && !GameManager.actionHappening)
            {
                _chargingThrow = false;
                StartCoroutine(RockThrowCoroutine());
            }
        }
    }

    private void ZoomIn(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && _chargingThrow == true)
        {
            _CinemachineZoomCamera.SetActive(true);
            _CinemachineZoomCamera.GetComponent<CinemachineFreeLook>().Follow = gameObject.GetComponent<InputController>().zoomFollowObject;
            _CinemachineZoomCamera.GetComponent<CinemachineFreeLook>().LookAt = gameObject.GetComponent<InputController>().zoomFollowObject;
        }
        else if (context.phase == InputActionPhase.Canceled && _chargingThrow == true)
        {
            _CinemachineZoomCamera.SetActive(false);
        }
    }

    private IEnumerator RockThrowCoroutine()
    {
        _FollowObject = _CinemachineFreeLookCamera.Follow;
        _LookObject = _CinemachineFreeLookCamera.LookAt;

        GameManager.actionHappening = true;

        _throwingObject = true;

        GameObject heavyRock = Instantiate(_ShootObject, _ShootPosition.transform.position, _ShootPosition.transform.rotation);

        _CinemachineFreeLookCamera.Follow = heavyRock.gameObject.transform;
        _CinemachineFreeLookCamera.LookAt = heavyRock.gameObject.transform;

        heavyRock.GetComponent<Rigidbody>().AddForce(_ShootPosition.transform.forward * _throwForce, ForceMode.Impulse);
        heavyRock.GetComponent<Rigidbody>().AddForce(_ShootPosition.transform.up / 5 * _throwForce, ForceMode.Impulse);

        yield return new WaitForSeconds(5);

        GameManager.endTurn = true;
        GameManager.actionHappening = false;
        _throwingObject = false;

        if (gameObject.GetComponent<InputController>()._isDead)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        _CinemachineZoomCamera.transform.position = _AimTarget.transform.position;
    }

    private void Update()
    {
        if (_chargingThrow)
        {
            _throwForce += _multiplier;
            _throwForce = Mathf.Clamp(_throwForce, 0, _maxThrowForce);
        }

        _ShootPosition.transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (GameManager.actionHappening)
        {
            _CinemachineZoomCamera.SetActive(false);
        }
    }
}
