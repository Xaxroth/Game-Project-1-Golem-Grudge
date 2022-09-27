using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] public static int numberOfPlayers = 4;
    [SerializeField] [Range(0, 3)] public static int playerRedGolems = 0;
    [SerializeField] [Range(0, 3)] public static int playerBlueGolems = 0;

    [SerializeField] [Range(0, 4)] public static int amountOfRedGolemsLeft = 4;
    [SerializeField] [Range(0, 4)] public static int amountOfBlueGolemsLeft = 4;

    public int golemIndex;
    public InputController currentGolem;
    public static int maxGolems = 12;

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
            StatusCheck();
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
        switch (playerTurn)
        {
            case 1:
                currentGolem.beingControlled = false;

                if (redGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                golemIndex++;
                golemIndex %= redGolemList.Count;
                currentGolem = redGolemList[golemIndex];

                for (int index = 0; index < redGolemList.Count; index++)
                {
                    redGolemList[index].hoveringPower = redGolemList[index].maximumHoveringPower;
                    redGolemList[index]._startPosition = redGolemList[index].transform.position;
                    redGolemList[index]._distanceMoved = 0;
                    redGolemList[index].canMove = true;
                }

                FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                FreeLookCamera.Follow = currentGolem.gameObject.transform;

                currentGolem.beingControlled = true;
                break;
            case 2:
                currentGolem.beingControlled = false;

                if (blueGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                golemIndex++;
                golemIndex %= blueGolemList.Count;
                currentGolem = blueGolemList[golemIndex];

                for (int index = 0; index < redGolemList.Count; index++)
                {
                    blueGolemList[index].hoveringPower = blueGolemList[index].maximumHoveringPower;
                    blueGolemList[index]._startPosition = blueGolemList[index].transform.position;
                    blueGolemList[index]._distanceMoved = 0;
                    blueGolemList[index].canMove = true;
                }

                FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                FreeLookCamera.Follow = currentGolem.gameObject.transform;

                currentGolem.beingControlled = true;
                break;
            case 3:
                currentGolem.beingControlled = false;

                if (greenGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                golemIndex++;
                golemIndex %= greenGolemList.Count;
                currentGolem = greenGolemList[golemIndex];

                for (int index = 0; index < redGolemList.Count; index++)
                {
                    greenGolemList[index].hoveringPower = greenGolemList[index].maximumHoveringPower;
                    greenGolemList[index]._startPosition = greenGolemList[index].transform.position;
                    greenGolemList[index]._distanceMoved = 0;
                    greenGolemList[index].canMove = true;
                }

                FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                FreeLookCamera.Follow = currentGolem.gameObject.transform;

                currentGolem.beingControlled = true;
                break;
            case 4:
                currentGolem.beingControlled = false;

                if (purpleGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                golemIndex++;
                golemIndex %= purpleGolemList.Count;
                currentGolem = purpleGolemList[golemIndex];

                for (int index = 0; index < redGolemList.Count; index++)
                {
                    purpleGolemList[index].hoveringPower = purpleGolemList[index].maximumHoveringPower;
                    purpleGolemList[index]._startPosition = purpleGolemList[index].transform.position;
                    purpleGolemList[index]._distanceMoved = 0;
                    purpleGolemList[index].canMove = true;
                }

                FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                FreeLookCamera.Follow = currentGolem.gameObject.transform;

                currentGolem.beingControlled = true;
                break;

        }
        //if (playerTurn == 1)
        //{
        //    currentGolem.beingControlled = false;
        //    currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

        //    golemIndex++;
        //    golemIndex %= redGolemList.Count;
        //    currentGolem = redGolemList[golemIndex];

        //    FreeLookCamera.LookAt = currentGolem.gameObject.transform;
        //    FreeLookCamera.Follow = currentGolem.gameObject.transform;

        //    currentGolem.beingControlled = true;

        //}
        //else if (playerTurn == 2)
        //{
        //    currentGolem.beingControlled = false;
        //    currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

        //    golemIndex++;
        //    golemIndex %= blueGolemList.Count;
        //    currentGolem = blueGolemList[golemIndex];

        //    FreeLookCamera.LookAt = currentGolem.gameObject.transform;
        //    FreeLookCamera.Follow = currentGolem.gameObject.transform;

        //    currentGolem.beingControlled = true;
        //}
        //else if (playerTurn == 3)
        //{
        //    currentGolem.beingControlled = false;
        //    currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

        //    golemIndex++;
        //    golemIndex %= greenGolemList.Count;
        //    currentGolem = greenGolemList[golemIndex];

        //    FreeLookCamera.LookAt = currentGolem.gameObject.transform;
        //    FreeLookCamera.Follow = currentGolem.gameObject.transform;

        //    currentGolem.beingControlled = true;
        //}
    }

    public static void WeaponUsed()
    {
        endTurn = true;
    }

    public void SwitchGolem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled && !actionHappening)
        {
            switch (playerTurn)
            {
                case 1:
                    currentGolem.beingControlled = false;
                    currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

                    golemIndex++;
                    golemIndex %= redGolemList.Count;
                    currentGolem = redGolemList[golemIndex];

                    FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                    FreeLookCamera.Follow = currentGolem.gameObject.transform;

                    currentGolem.beingControlled = true;
                    break;
                case 2:
                    currentGolem.beingControlled = false;
                    currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

                    golemIndex++;
                    golemIndex %= blueGolemList.Count;
                    currentGolem = blueGolemList[golemIndex];

                    FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                    FreeLookCamera.Follow = currentGolem.gameObject.transform;

                    currentGolem.beingControlled = true;
                    break;
                case 3:
                    currentGolem.beingControlled = false;
                    currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

                    golemIndex++;
                    golemIndex %= greenGolemList.Count;
                    currentGolem = greenGolemList[golemIndex];

                    FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                    FreeLookCamera.Follow = currentGolem.gameObject.transform;

                    currentGolem.beingControlled = true;
                    break;
                case 4:
                    currentGolem.beingControlled = false;
                    currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

                    golemIndex++;
                    golemIndex %= purpleGolemList.Count;
                    currentGolem = purpleGolemList[golemIndex];

                    FreeLookCamera.LookAt = currentGolem.gameObject.transform;
                    FreeLookCamera.Follow = currentGolem.gameObject.transform;

                    currentGolem.beingControlled = true;
                    break;


            }
            //if (playerTurn == 1)
            //{
            //    currentGolem.beingControlled = false;
            //    currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

            //    golemIndex++;
            //    golemIndex %= redGolemList.Count;
            //    currentGolem = redGolemList[golemIndex];

            //    FreeLookCamera.LookAt = currentGolem.gameObject.transform;
            //    FreeLookCamera.Follow = currentGolem.gameObject.transform;

            //    currentGolem.beingControlled = true;
            //}
            //if (playerTurn == 2)
            //{
            //    currentGolem.beingControlled = false;
            //    currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

            //    golemIndex++;
            //    golemIndex %= blueGolemList.Count;
            //    currentGolem = blueGolemList[golemIndex];

            //    FreeLookCamera.LookAt = currentGolem.gameObject.transform;
            //    FreeLookCamera.Follow = currentGolem.gameObject.transform;

            //    currentGolem.beingControlled = true;
            //}
        }
    }

    void Update()
    {
        InitializeGame();

        //if (blueGolemList.Count == 0)
        //{
        //    winnerText.text = players[0].ToString();
        //    actionHappening = true;
        //    gameOverObject.SetActive(true);
        //}

        //if (redGolemList.Count == 0)
        //{
        //    winnerText.text = players[1].ToString();
        //    actionHappening = true;
        //    gameOverObject.SetActive(true);
        //}

        //if (numberOfPlayers == 3 && redGolemList.Count == 0 && blueGolemList.Count == 0)
        //{
        //    winnerText.text = players[2].ToString();
        //    actionHappening = true;
        //    gameOverObject.SetActive(true);
        //}
        //else if (numberOfPlayers == 4 && redGolemList.Count == 0 && blueGolemList.Count == 0 && purpleGolemList.Count == 0)
        //{
        //    winnerText.text = players[2].ToString();
        //    actionHappening = true;
        //    gameOverObject.SetActive(true);
        //}

        //if (redGolemList.Count == 0 && blueGolemList.Count == 0 && greenGolemList.Count == 0)
        //{
        //    winnerText.text = players[2].ToString();
        //    actionHappening = true;
        //    gameOverObject.SetActive(true);
        //}
    }

    public void StatusCheck()
    {
        switch (numberOfPlayers)
        {
            case 2:
                if (blueGolemList.Count == 0)
                {
                    winnerText.text = players[0].ToString();
                    actionHappening = true;
                    gameOverObject.SetActive(true);
                }

                if (redGolemList.Count == 0)
                {
                    winnerText.text = players[1].ToString();
                    actionHappening = true;
                    gameOverObject.SetActive(true);
                }
                break;
            case 3:
                if (blueGolemList.Count == 0 && greenGolemList.Count == 0)
                {
                    winnerText.text = players[0].ToString();
                    actionHappening = true;
                    gameOverObject.SetActive(true);
                }
                if (redGolemList.Count == 0 && greenGolemList.Count == 0)
                {
                    winnerText.text = players[1].ToString();
                    actionHappening = true;
                    gameOverObject.SetActive(true);
                }
                if (redGolemList.Count == 0 && blueGolemList.Count == 0)
                {
                    winnerText.text = players[2].ToString();
                    actionHappening = true;
                    gameOverObject.SetActive(true);
                }
                break;
            case 4:
                if (blueGolemList.Count == 0 && greenGolemList.Count == 0 && purpleGolemList.Count == 0)
                {
                    winnerText.text = players[0].ToString();
                    actionHappening = true;
                    gameOverObject.SetActive(true);
                }
                if (redGolemList.Count == 0 && greenGolemList.Count == 0 && purpleGolemList.Count == 0)
                {
                    winnerText.text = players[1].ToString();
                    actionHappening = true;
                    gameOverObject.SetActive(true);
                }
                if (redGolemList.Count == 0 && blueGolemList.Count == 0 && purpleGolemList.Count == 0)
                {
                    winnerText.text = players[2].ToString();
                    actionHappening = true;
                    gameOverObject.SetActive(true);
                }
                if (redGolemList.Count == 0 && greenGolemList.Count == 0 && purpleGolemList.Count == 0)
                {
                    winnerText.text = players[3].ToString();
                    actionHappening = true;
                    gameOverObject.SetActive(true);
                }
                break;

        }
    }

    private void InitializeGame()
    {
        if (!listSorted)
        {
            Debug.Log("INITIALIZED GAME");
            allGolems = allGolems.OrderBy(o => o.name).ToList();

            redGolemList = allGolems.Where(o => o.playerNumber == 1).ToList();
            greenGolemList = allGolems.Where(o => o.playerNumber == 3).ToList();
            blueGolemList = allGolems.Where(o => o.playerNumber == 2).ToList();
            purpleGolemList = allGolems.Where(o => o.playerNumber == 4).ToList();

            currentGolem = redGolemList[0];

            currentGolem.beingControlled = true;

            greenGolemList[0].gameObject.SetActive(false);
            greenGolemList[1].gameObject.SetActive(false);
            greenGolemList[2].gameObject.SetActive(false);
            greenGolemList[3].gameObject.SetActive(false);

            purpleGolemList[0].gameObject.SetActive(false);
            purpleGolemList[1].gameObject.SetActive(false);
            purpleGolemList[2].gameObject.SetActive(false);
            purpleGolemList[3].gameObject.SetActive(false);

            if (numberOfPlayers == 3)
            {
                maxGolems = 12;
                greenGolemList[0].gameObject.SetActive(true);
                greenGolemList[1].gameObject.SetActive(true);
                greenGolemList[2].gameObject.SetActive(true);
                greenGolemList[3].gameObject.SetActive(true);
            }

            if (numberOfPlayers == 4)
            {
                maxGolems = 16;

                greenGolemList[0].gameObject.SetActive(true);
                greenGolemList[1].gameObject.SetActive(true);
                greenGolemList[2].gameObject.SetActive(true);
                greenGolemList[3].gameObject.SetActive(true);

                purpleGolemList[0].gameObject.SetActive(true);
                purpleGolemList[1].gameObject.SetActive(true);
                purpleGolemList[2].gameObject.SetActive(true);
                purpleGolemList[3].gameObject.SetActive(true);
            }

            listSorted = true;
        }
    }
}
