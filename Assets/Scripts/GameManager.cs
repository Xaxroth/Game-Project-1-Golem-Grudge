using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] public static int numberOfPlayers = 2;
    [SerializeField] [Range(0, 7)] public static int golemNumber = 0;
    [SerializeField] [Range(1, 8)] private static int amountOfGolems = 7;
    [SerializeField] public CinemachineFreeLook FreeLookCamera;
    public static bool actionHappening = false;
    public InputController[] golems;
    public static bool endTurn = false;
    [SerializeField] public static int playerTurn = 1;
    [SerializeField] public static int turnCountdown = 30;

    // Start is called before the first frame update
    void Start()
    {
        playerTurn = numberOfPlayers;
        StartCoroutine(countDownTimer());
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
            StartCoroutine(countDownTimer());
            Debug.Log("BOB SAGET ENDED TURN IN 1990");
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
        Debug.Log("Current Player is: " + playerTurn);
        turnCountdown = 30;
    }

    public static void WeaponUsed()
    {
        endTurn = true;
    }

    public void SwitchGolem(InputAction.CallbackContext context)
    {
        if (golemNumber == amountOfGolems && context.phase == InputActionPhase.Canceled)
        {
            golems[golemNumber].gameObject.GetComponent<InputController>().beingControlled = false;
            golemNumber = 0;
        }
        else if (golemNumber < amountOfGolems && context.phase == InputActionPhase.Canceled)
        {
            golems[golemNumber].gameObject.GetComponent<InputController>().beingControlled = false;
            golems[golemNumber].gameObject.GetComponent<InputController>().hoveringPower = golems[golemNumber].gameObject.GetComponent<InputController>().maximumHoveringPower;
            golemNumber++;
        }

        FreeLookCamera.LookAt = golems[golemNumber].gameObject.transform;
        FreeLookCamera.Follow = golems[golemNumber].gameObject.transform;
        golems[golemNumber].gameObject.GetComponent<InputController>().beingControlled = true;
        Debug.Log("The golem number is: " + golemNumber);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(golemNumber);
    }
}
