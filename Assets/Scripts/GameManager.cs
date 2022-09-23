using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

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
    public List<InputController> greenGolemList;
    public List<InputController> purpleGolemList;

    public List<InputController> allGolems;

    public static bool actionHappening = false;
    public static bool endTurn = false;
    public static bool gameStart = true;
    private bool listSorted = false;

    private string[] players = new string[] { "Red", "Blue", "Green", "Purple" };

    public TextMeshProUGUI winnerText;

    [SerializeField] private GameObject gameOverObject;
    [SerializeField] private GameObject victoryObject;
    [SerializeField] private GameObject pauseObject;

    [SerializeField] public static int playerTurn = 1;
    [SerializeField] public static int turnCountdown = 31;

    void Awake()
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
            currentGolem.beingControlled = false;
            currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

            golemIndex++;
            golemIndex %= redGolemList.Count;
            currentGolem = redGolemList[golemIndex];

            FreeLookCamera.LookAt = currentGolem.gameObject.transform;
            FreeLookCamera.Follow = currentGolem.gameObject.transform;

            currentGolem.beingControlled = true;

        }
        else if (playerTurn == 2)
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

    public static void WeaponUsed()
    {
        endTurn = true;
    }

    public void SwitchGolem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled && !actionHappening)
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
        InitializeGame();

        if (blueGolemList.Count == 0)
        {
            winnerText.text = players[0].ToString();
            gameOverObject.SetActive(true);
        }

        if (redGolemList.Count == 0)
        {
            winnerText.text = players[1].ToString();
            gameOverObject.SetActive(true);
        }

        //if (pauseObject.activeInHierarchy)
        //{
        //    Time.timeScale = 0;
        //}
        //else
        //{
        //    Time.timeScale = 1;
        //}
    }

    private void InitializeGame()
    {
        if (allGolems.Count == maxGolems && !listSorted)
        {
            allGolems = allGolems.OrderBy(o => o.name).ToList();

            redGolemList = allGolems.Where(o => o.playerNumber == 1).ToList();
            blueGolemList = allGolems.Where(o => o.playerNumber == 2).ToList();
            greenGolemList = allGolems.Where(o => o.playerNumber == 3).ToList();
            purpleGolemList = allGolems.Where(o => o.playerNumber == 4).ToList();

            currentGolem = redGolemList[0];

            currentGolem.beingControlled = true;

            //greenGolemList[0].gameObject.SetActive(false);
            //greenGolemList[1].gameObject.SetActive(false);
            //greenGolemList[2].gameObject.SetActive(false);
            //greenGolemList[3].gameObject.SetActive(false);

            //purpleGolemList[0].gameObject.SetActive(false);
            //purpleGolemList[1].gameObject.SetActive(false);
            //purpleGolemList[2].gameObject.SetActive(false);
            //purpleGolemList[3].gameObject.SetActive(false);

            //if (numberOfPlayers == 3 || numberOfPlayers == 4)
            //{
            //    maxGolems = 12;
            //    greenGolemList[0].gameObject.SetActive(true);
            //    greenGolemList[1].gameObject.SetActive(true);
            //    greenGolemList[2].gameObject.SetActive(true);
            //    greenGolemList[3].gameObject.SetActive(true);
            //}

            //if (numberOfPlayers == 4)
            //{
            //    maxGolems = 16;
            //    purpleGolemList[0].gameObject.SetActive(true);
            //    purpleGolemList[1].gameObject.SetActive(true);
            //    purpleGolemList[2].gameObject.SetActive(true);
            //    purpleGolemList[3].gameObject.SetActive(true);
            //}

            listSorted = true;
        }
    }
}
