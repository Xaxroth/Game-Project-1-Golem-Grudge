using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineFreeLook FreeLookCamera;

    [Header("Player Controls")]
    public InputController currentGolem;

    public int golemIndex;

    public List<InputController> redGolemList;
    public List<InputController> blueGolemList;
    public List<InputController> greenGolemList;
    public List<InputController> purpleGolemList;

    public List<InputController> allGolems;

    [Header("User Interface")]
    private string[] players = new string[] { "Fire", "Spirit", "Life", "Death" };

    [SerializeField] private UITextScript UITextController;

    [SerializeField] private TextMeshProUGUI winnerText;

    [SerializeField] private GameObject gameOverObject;
    [SerializeField] private GameObject victoryObject;
    [SerializeField] private GameObject pauseObject;

    [SerializeField] private bool listSorted = false;

    public static int numberOfPlayers = 4;
    public static int maxGolems = 12;
    public static int playerTurn = 1;
    public static int turnDuration = 30;
    public static int maxTurnDuration = 30;

    public static bool actionHappening = false;
    public static bool endTurn = false;
    public static bool gameStart = true;
    public static bool gamePaused;

    private void Awake()
    {
        ResetConditions();

        if (gameStart)
        {
            playerTurn = 1;

            StartCoroutine(countDownTimer());
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    public IEnumerator countDownTimer()
    {
        endTurn = false;

        if (!actionHappening)
        {
            turnDuration--;
        }

        yield return new WaitForSeconds(1);

        if (turnDuration == 0 || endTurn)
        {
            StatusCheck();
            NewPlayer();
            ResetPlayer();
            StartCoroutine(countDownTimer());
            UITextController.TurnChecker();
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

        turnDuration = maxTurnDuration;
    }

    public void ResetPlayer()
    {
        currentGolem.beingControlled = false;

        golemIndex++;

        switch (playerTurn)
        {
            case 1:
                if (redGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                golemIndex %= redGolemList.Count;
                currentGolem = redGolemList[golemIndex];

                for (int index = 0; index < redGolemList.Count; index++)
                {
                    redGolemList[index].hoveringPower = redGolemList[index].maximumHoveringPower;
                    redGolemList[index]._startPosition = redGolemList[index].transform.position;
                    redGolemList[index]._distanceMoved = 0;
                    redGolemList[index].canMove = true;
                }
                break;
            case 2:
                if (blueGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                golemIndex %= blueGolemList.Count;
                currentGolem = blueGolemList[golemIndex];

                for (int index = 0; index < blueGolemList.Count; index++)
                {
                    blueGolemList[index].hoveringPower = blueGolemList[index].maximumHoveringPower;
                    blueGolemList[index]._startPosition = blueGolemList[index].transform.position;
                    blueGolemList[index]._distanceMoved = 0;
                    blueGolemList[index].canMove = true;
                }
                break;
            case 3:
                if (greenGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                golemIndex %= greenGolemList.Count;
                currentGolem = greenGolemList[golemIndex];

                for (int index = 0; index < greenGolemList.Count; index++)
                {
                    greenGolemList[index].hoveringPower = greenGolemList[index].maximumHoveringPower;
                    greenGolemList[index]._startPosition = greenGolemList[index].transform.position;
                    greenGolemList[index]._distanceMoved = 0;
                    greenGolemList[index].canMove = true;
                }
                break;
            case 4:
                if (purpleGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                golemIndex %= purpleGolemList.Count;
                currentGolem = purpleGolemList[golemIndex];

                for (int index = 0; index < purpleGolemList.Count; index++)
                {
                    purpleGolemList[index].hoveringPower = purpleGolemList[index].maximumHoveringPower;
                    purpleGolemList[index]._startPosition = purpleGolemList[index].transform.position;
                    purpleGolemList[index]._distanceMoved = 0;
                    purpleGolemList[index].canMove = true;
                }
                break;
        }

        FreeLookCamera.LookAt = currentGolem.gameObject.transform;
        FreeLookCamera.Follow = currentGolem.gameObject.transform;

        currentGolem.beingControlled = true;
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!gamePaused)
            {
                AudioListener.volume = 0;

                Time.timeScale = 0;
                pauseObject.SetActive(true);
                gamePaused = true;
            }
            else
            {
                AudioListener.volume = 1;

                Time.timeScale = 1;
                pauseObject.SetActive(false);
                gamePaused = false;
            }
        }
    }

    public void SwitchGolem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled && !actionHappening)
        {
            currentGolem.beingControlled = false;
            currentGolem.hoveringPower = currentGolem.maximumHoveringPower;

            golemIndex++;

            switch (playerTurn)
            {
                case 1:
                    golemIndex %= redGolemList.Count;
                    currentGolem = redGolemList[golemIndex];
                    break;
                case 2:
                    golemIndex %= blueGolemList.Count;
                    currentGolem = blueGolemList[golemIndex];
                    break;
                case 3:
                    golemIndex %= greenGolemList.Count;
                    currentGolem = greenGolemList[golemIndex];
                    break;
                case 4:
                    golemIndex %= purpleGolemList.Count;
                    currentGolem = purpleGolemList[golemIndex];
                    break;
            }

            FreeLookCamera.LookAt = currentGolem.gameObject.transform;
            FreeLookCamera.Follow = currentGolem.gameObject.transform;

            currentGolem.beingControlled = true;
        }
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
                if (redGolemList.Count == 0 && greenGolemList.Count == 0 && blueGolemList.Count == 0)
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
            UITextController = gameObject.GetComponent<UITextScript>();
            UITextController.TurnChecker();

            allGolems = allGolems.OrderBy(o => o.name).ToList();

            redGolemList = allGolems.Where(o => o.playerNumber == 1).ToList();
            greenGolemList = allGolems.Where(o => o.playerNumber == 3).ToList();
            blueGolemList = allGolems.Where(o => o.playerNumber == 2).ToList();
            purpleGolemList = allGolems.Where(o => o.playerNumber == 4).ToList();

            currentGolem = redGolemList[0];

            currentGolem.beingControlled = true;

            for (int index = 0; index < greenGolemList.Count; index++)
            {
                greenGolemList[index].gameObject.SetActive(false);
            }

            for (int index = 0; index < purpleGolemList.Count; index++)
            {
                purpleGolemList[index].gameObject.SetActive(false);
            }

            if (numberOfPlayers == 3)
            {
                maxGolems = 12;

                for (int index = 0; index < greenGolemList.Count; index++)
                {
                    greenGolemList[index].gameObject.SetActive(true);
                }
            }

            if (numberOfPlayers == 4)
            {
                maxGolems = 16;

                for (int index = 0; index < greenGolemList.Count; index++)
                {
                    greenGolemList[index].gameObject.SetActive(true);
                }

                for (int index = 0; index < purpleGolemList.Count; index++)
                {
                    purpleGolemList[index].gameObject.SetActive(true);
                }
            }

            AudioListener.volume = 1;

            Time.timeScale = 1;
            pauseObject.SetActive(false);
            gamePaused = false;

            listSorted = true;
        }
    }

    private void ResetConditions()
    {
        maxGolems = 12;
        playerTurn = 1;
        turnDuration = 30;
        maxTurnDuration = 30;

        actionHappening = false;
        endTurn = false;
        gameStart = true;
        gamePaused = false;
}
}
