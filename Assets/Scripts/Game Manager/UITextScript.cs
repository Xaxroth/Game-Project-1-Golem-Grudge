using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITextScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _CountDownText;

    [SerializeField] private GameObject _RedText;
    [SerializeField] private GameObject _BlueText;
    [SerializeField] private GameObject _GreenText;
    [SerializeField] private GameObject _PurpleText;

    void Update()
    {
        _CountDownText.text = GameManager.turnDuration.ToString();
    }

    public void TurnChecker()
    {
        _RedText.SetActive(false);
        _BlueText.SetActive(false);
        _GreenText.SetActive(false);
        _PurpleText.SetActive(false);

        switch (GameManager.playerTurn)
        {
            case 1:
                _RedText.SetActive(true);
                break;
            case 2:
                _BlueText.SetActive(true);
                break;
            case 3:
                _GreenText.SetActive(true);
                break;
            case 4:
                _PurpleText.SetActive(true);
                break;
        }
    }
}
