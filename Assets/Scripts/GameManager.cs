using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] public static int numberOfPlayers = 2;
    [SerializeField] [Range(0, 3)] public static int playerRedGolems = 0;
    [SerializeField] [Range(0, 3)] public static int playerBlueGolems = 0;
    [SerializeField] [Range(0, 4)] private static int amountOfGolems = 4;

    [SerializeField] public CinemachineFreeLook FreeLookCamera;

    public InputController[] redGolem;
    public InputController[] blueGolem;

    public static bool actionHappening = false;
    public static bool endTurn = false;
    public static bool gameStart = true;

    [SerializeField] public static int playerTurn = 1;
    [SerializeField] public static int turnCountdown = 31;

    void Start()
    {
        if (gameStart)
        {
            playerTurn = 1;
            StartCoroutine(countDownTimer());
        }
    }

    public IEnumerator countDownTimer()
    {
        endTurn = false;

        if (!actionHappening)
        {
            turnCountdown--;
        }

        yield return new WaitForSeconds(1);

        if (turnCountdown == 0 || endTurn)
        {
            NewPlayer();
            ResetPlayer();
            StartCoroutine(countDownTimer());
        }
        else
        {
            StartCoroutine(countDownTimer());
        }
    }

    public static void NewPlayer()
    {
        if (playerTurn == numberOfPlayers)
        {
            playerTurn = 1;
        }
        else
        {
            playerTurn++;
        }

        turnCountdown = 31;
    }

    public void ResetPlayer()
    {
        if (playerTurn == 1)
        {
            blueGolem[playerBlueGolems].gameObject.GetComponent<InputController>().beingControlled = false;
            playerRedGolems = 0;

            FreeLookCamera.LookAt = redGolem[playerRedGolems].gameObject.transform;
            FreeLookCamera.Follow = redGolem[playerRedGolems].gameObject.transform;

            redGolem[playerRedGolems].gameObject.GetComponent<InputController>().beingControlled = true;
        }
        else if (playerTurn == 2)
        {
            redGolem[playerRedGolems].gameObject.GetComponent<InputController>().beingControlled = false;
            playerBlueGolems = 0;

            FreeLookCamera.LookAt = blueGolem[playerBlueGolems].gameObject.transform;
            FreeLookCamera.Follow = blueGolem[playerBlueGolems].gameObject.transform;

            blueGolem[playerBlueGolems].gameObject.GetComponent<InputController>().beingControlled = true;
        }
    }

    public static void WeaponUsed()
    {
        endTurn = true;
    }

    public void SwitchGolem(InputAction.CallbackContext context)
    {
        if (playerTurn == 1)
        {
            if (playerRedGolems == amountOfGolems && context.phase == InputActionPhase.Canceled)
            {
                redGolem[playerRedGolems].gameObject.GetComponent<InputController>().beingControlled = false;
                playerRedGolems = 0;
            }
            else if (playerRedGolems < amountOfGolems && context.phase == InputActionPhase.Canceled)
            {
                redGolem[playerRedGolems].gameObject.GetComponent<InputController>().beingControlled = false;
                redGolem[playerRedGolems].gameObject.GetComponent<InputController>().hoveringPower = redGolem[playerRedGolems].gameObject.GetComponent<InputController>().maximumHoveringPower;

                playerRedGolems++;
                playerRedGolems %= amountOfGolems;
            }

            FreeLookCamera.LookAt = redGolem[playerRedGolems].gameObject.transform;
            FreeLookCamera.Follow = redGolem[playerRedGolems].gameObject.transform;

            redGolem[playerRedGolems].gameObject.GetComponent<InputController>().beingControlled = true;
        }
        else
        {
            if (playerBlueGolems == amountOfGolems && context.phase == InputActionPhase.Canceled)
            {
                blueGolem[playerBlueGolems].gameObject.GetComponent<InputController>().beingControlled = false;
                playerBlueGolems = 0;
            }
            else if (playerBlueGolems < amountOfGolems && context.phase == InputActionPhase.Canceled)
            {
                blueGolem[playerBlueGolems].gameObject.GetComponent<InputController>().beingControlled = false;
                blueGolem[playerBlueGolems].gameObject.GetComponent<InputController>().hoveringPower = blueGolem[playerBlueGolems].gameObject.GetComponent<InputController>().maximumHoveringPower;

                playerBlueGolems++;
                playerBlueGolems %= amountOfGolems;
            }

            FreeLookCamera.LookAt = blueGolem[playerBlueGolems].gameObject.transform;
            FreeLookCamera.Follow = blueGolem[playerBlueGolems].gameObject.transform;

            blueGolem[playerBlueGolems].gameObject.GetComponent<InputController>().beingControlled = true;
        }
    }

    void Update()
    {

    }
}
