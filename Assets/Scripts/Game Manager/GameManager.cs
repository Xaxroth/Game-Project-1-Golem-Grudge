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
    [SerializeField] private CinemachineFreeLook _FreeLookCamera;

    [Header("Player Controls")]
    public InputController CurrentGolem;

    [SerializeField] private int _golemIndex;

    public List<InputController> RedGolemList;
    public List<InputController> BlueGolemList;
    public List<InputController> GreenGolemList;
    public List<InputController> PurpleGolemList;

    public List<InputController> AllGolems;

    [Header("User Interface")]
    private string[] _players = new string[] { "Fire", "Spirit", "Life", "Death" };

    [SerializeField] private UITextScript UITextController;

    [SerializeField] private TextMeshProUGUI _WinnerText;

    [SerializeField] private GameObject _GameOverObject;
    [SerializeField] private GameObject _VictoryObject;
    [SerializeField] private GameObject _PauseObject;

    [SerializeField] private bool _listSorted = false;

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
        CurrentGolem.beingControlled = false;

        _golemIndex++;

        switch (playerTurn)
        {
            case 1:
                if (RedGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                _golemIndex %= RedGolemList.Count;
                CurrentGolem = RedGolemList[_golemIndex];

                for (int index = 0; index < RedGolemList.Count; index++)
                {
                    RedGolemList[index].hoveringPower = RedGolemList[index].maximumHoveringPower;
                    RedGolemList[index]._startPosition = RedGolemList[index].transform.position;
                    RedGolemList[index]._distanceMoved = 0;
                    RedGolemList[index].canMove = true;
                }
                break;
            case 2:
                if (BlueGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                _golemIndex %= BlueGolemList.Count;
                CurrentGolem = BlueGolemList[_golemIndex];

                for (int index = 0; index < BlueGolemList.Count; index++)
                {
                    BlueGolemList[index].hoveringPower = BlueGolemList[index].maximumHoveringPower;
                    BlueGolemList[index]._startPosition = BlueGolemList[index].transform.position;
                    BlueGolemList[index]._distanceMoved = 0;
                    BlueGolemList[index].canMove = true;
                }
                break;
            case 3:
                if (GreenGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                _golemIndex %= GreenGolemList.Count;
                CurrentGolem = GreenGolemList[_golemIndex];

                for (int index = 0; index < GreenGolemList.Count; index++)
                {
                    GreenGolemList[index].hoveringPower = GreenGolemList[index].maximumHoveringPower;
                    GreenGolemList[index]._startPosition = GreenGolemList[index].transform.position;
                    GreenGolemList[index]._distanceMoved = 0;
                    GreenGolemList[index].canMove = true;
                }
                break;
            case 4:
                if (PurpleGolemList.Count == 0)
                {
                    NewPlayer();
                    ResetPlayer();
                    break;
                }

                _golemIndex %= PurpleGolemList.Count;
                CurrentGolem = PurpleGolemList[_golemIndex];

                for (int index = 0; index < PurpleGolemList.Count; index++)
                {
                    PurpleGolemList[index].hoveringPower = PurpleGolemList[index].maximumHoveringPower;
                    PurpleGolemList[index]._startPosition = PurpleGolemList[index].transform.position;
                    PurpleGolemList[index]._distanceMoved = 0;
                    PurpleGolemList[index].canMove = true;
                }
                break;
        }

        _FreeLookCamera.LookAt = CurrentGolem.gameObject.transform;
        _FreeLookCamera.Follow = CurrentGolem.gameObject.transform;

        CurrentGolem.beingControlled = true;
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!gamePaused)
            {
                AudioListener.volume = 0;

                Time.timeScale = 0;
                _PauseObject.SetActive(true);
                gamePaused = true;
            }
            else
            {
                AudioListener.volume = 1;

                Time.timeScale = 1;
                _PauseObject.SetActive(false);
                gamePaused = false;
            }
        }
    }

    public void SwitchGolem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled && !actionHappening)
        {
            CurrentGolem.beingControlled = false;
            CurrentGolem.hoveringPower = CurrentGolem.maximumHoveringPower;

            _golemIndex++;

            switch (playerTurn)
            {
                case 1:
                    _golemIndex %= RedGolemList.Count;
                    CurrentGolem = RedGolemList[_golemIndex];
                    break;
                case 2:
                    _golemIndex %= BlueGolemList.Count;
                    CurrentGolem = BlueGolemList[_golemIndex];
                    break;
                case 3:
                    _golemIndex %= GreenGolemList.Count;
                    CurrentGolem = GreenGolemList[_golemIndex];
                    break;
                case 4:
                    _golemIndex %= PurpleGolemList.Count;
                    CurrentGolem = PurpleGolemList[_golemIndex];
                    break;
            }

            _FreeLookCamera.LookAt = CurrentGolem.gameObject.transform;
            _FreeLookCamera.Follow = CurrentGolem.gameObject.transform;

            CurrentGolem.beingControlled = true;
        }
    }

    public void StatusCheck()
    {
        switch (numberOfPlayers)
        {
            case 2:
                if (BlueGolemList.Count == 0)
                {
                    _WinnerText.text = _players[0].ToString();
                    actionHappening = true;
                    _GameOverObject.SetActive(true);
                }

                if (RedGolemList.Count == 0)
                {
                    _WinnerText.text = _players[1].ToString();
                    actionHappening = true;
                    _GameOverObject.SetActive(true);
                }
                break;
            case 3:
                if (BlueGolemList.Count == 0 && GreenGolemList.Count == 0)
                {
                    _WinnerText.text = _players[0].ToString();
                    actionHappening = true;
                    _GameOverObject.SetActive(true);
                }
                if (RedGolemList.Count == 0 && GreenGolemList.Count == 0)
                {
                    _WinnerText.text = _players[1].ToString();
                    actionHappening = true;
                    _GameOverObject.SetActive(true);
                }
                if (RedGolemList.Count == 0 && BlueGolemList.Count == 0)
                {
                    _WinnerText.text = _players[2].ToString();
                    actionHappening = true;
                    _GameOverObject.SetActive(true);
                }
                break;
            case 4:
                if (BlueGolemList.Count == 0 && GreenGolemList.Count == 0 && PurpleGolemList.Count == 0)
                {
                    _WinnerText.text = _players[0].ToString();
                    actionHappening = true;
                    _GameOverObject.SetActive(true);
                }
                if (RedGolemList.Count == 0 && GreenGolemList.Count == 0 && PurpleGolemList.Count == 0)
                {
                    _WinnerText.text = _players[1].ToString();
                    actionHappening = true;
                    _GameOverObject.SetActive(true);
                }
                if (RedGolemList.Count == 0 && BlueGolemList.Count == 0 && PurpleGolemList.Count == 0)
                {
                    _WinnerText.text = _players[2].ToString();
                    actionHappening = true;
                    _GameOverObject.SetActive(true);
                }
                if (RedGolemList.Count == 0 && GreenGolemList.Count == 0 && BlueGolemList.Count == 0)
                {
                    _WinnerText.text = _players[3].ToString();
                    actionHappening = true;
                    _GameOverObject.SetActive(true);
                }
                break;
        }
    }

    private void InitializeGame()
    {
        if (!_listSorted)
        {
            UITextController = gameObject.GetComponent<UITextScript>();
            UITextController.TurnChecker();

            AllGolems = AllGolems.OrderBy(o => o.name).ToList();

            RedGolemList = AllGolems.Where(o => o.playerNumber == 1).ToList();
            GreenGolemList = AllGolems.Where(o => o.playerNumber == 3).ToList();
            BlueGolemList = AllGolems.Where(o => o.playerNumber == 2).ToList();
            PurpleGolemList = AllGolems.Where(o => o.playerNumber == 4).ToList();

            CurrentGolem = RedGolemList[0];

            CurrentGolem.beingControlled = true;

            for (int index = 0; index < GreenGolemList.Count; index++)
            {
                GreenGolemList[index].gameObject.SetActive(false);
            }

            for (int index = 0; index < PurpleGolemList.Count; index++)
            {
                PurpleGolemList[index].gameObject.SetActive(false);
            }

            if (numberOfPlayers == 3)
            {
                maxGolems = 12;

                for (int index = 0; index < GreenGolemList.Count; index++)
                {
                    GreenGolemList[index].gameObject.SetActive(true);
                }
            }

            if (numberOfPlayers == 4)
            {
                maxGolems = 16;

                for (int index = 0; index < GreenGolemList.Count; index++)
                {
                    GreenGolemList[index].gameObject.SetActive(true);
                }

                for (int index = 0; index < PurpleGolemList.Count; index++)
                {
                    PurpleGolemList[index].gameObject.SetActive(true);
                }
            }

            AudioListener.volume = 1;

            Time.timeScale = 1;
            _PauseObject.SetActive(false);
            gamePaused = false;

            _listSorted = true;
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
