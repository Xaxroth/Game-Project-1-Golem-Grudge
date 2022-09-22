using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] public static int numberOfPlayers = 2;
    [SerializeField] [Range(0, 3)] public static int playerRedGolems = 0;
    [SerializeField] [Range(0, 3)] public static int playerBlueGolems = 0;

    [SerializeField] [Range(0, 4)] public static int amountOfRedGolemsLeft = 4;
    [SerializeField] [Range(0, 4)] public static int amountOfBlueGolemsLeft = 4;

    public int golemIndex;
    public InputController currentGolem;
    public int maxGolems = 8;

    [SerializeField] public CinemachineFreeLook FreeLookCamera;

    public InputController[] redGolem;
    public InputController[] blueGolem;

    public List<InputController> redGolemList;
    public List<InputController> blueGolemList;

    public List<InputController> allGolems;

    public static bool actionHappening = false;
    public static bool endTurn = false;
    public static bool gameStart = true;
    private bool listSorted = false;

    [SerializeField] public static int playerTurn = 1;
    [SerializeField] public static int turnCountdown = 31;

    void Awake()
    {
        //redGolems = new List<GameObject>();
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
        if (context.phase == InputActionPhase.Canceled)
        {
            if (playerTurn == 1)
            {
                currentGolem.beingControlled = false;
                currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

                golemIndex++;
                golemIndex %= redGolemList.Count;
                currentGolem = redGolemList[golemIndex];

                FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                FreeLookCamera.Follow = currentGolem.gameObject.transform;

                currentGolem.beingControlled = true;
            }
            else
            {
                currentGolem.beingControlled = false;
                currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

                golemIndex++;
                golemIndex %= blueGolemList.Count;
                currentGolem = blueGolemList[golemIndex];

                FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                FreeLookCamera.Follow = currentGolem.gameObject.transform;

                currentGolem.beingControlled = true;
            }
        }
    }

    void Update()
    {
        if (allGolems.Count == maxGolems && !listSorted)
        {
            allGolems = allGolems.OrderBy(o => o.name).ToList();
           
            redGolemList = allGolems.Where(o => o.playerNumber == 1).ToList();
            blueGolemList = allGolems.Where(o => o.playerNumber == 2).ToList();

            currentGolem = redGolemList[0];

            listSorted = true;
        }
    }
}
