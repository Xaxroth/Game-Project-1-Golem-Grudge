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
    [SerializeField] public CinemachineFreeLook FreeLookCamera;

    [Header("Player Controls")]
    public InputController currentGolem;

    public int golemIndex;
    public static int maxGolems = 12;

    public List<InputController> redGolemList;
    public List<InputController> blueGolemList;
    public List<InputController> greenGolemList;
    public List<InputController> purpleGolemList;

    public List<InputController> allGolems;

    [SerializeField] private bool listSorted = false;

    [Header("User Interface")]
    private string[] players = new string[] { "Fire", "Spirit", "Life", "Death" };

    private UITextScript UITextController;

    public TextMeshProUGUI winnerText;

    [SerializeField] private GameObject gameOverObject;
    [SerializeField] private GameObject victoryObject;
    [SerializeField] private GameObject pauseObject;

    public static int numberOfPlayers = 4;
    public static int playerTurn = 1;
    public static int turnDuration = 30;
    public static int maxTurnDuration = 30;
    public static bool actionHappening = false;
    public static bool endTurn = false;
    public static bool gameStart = true;

    public static bool gamePaused;

    private void Awake()
    {
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
            UITextController.TurnChecker();
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

        turnDuration = maxTurnDuration;
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

                for (int index = 0; index < blueGolemList.Count; index++)
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

                for (int index = 0; index < greenGolemList.Count; index++)
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

                for (int index = 0; index < purpleGolemList.Count; index++)
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

            listSorted = true;
        }
    }
}
