using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITextScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;

    [SerializeField] private GameObject redText;
    [SerializeField] private GameObject blueText;
    [SerializeField] private GameObject greenText;
    [SerializeField] private GameObject purpleText;

    void Update()
    {
        countDownText.text = GameManager.turnDuration.ToString();
    }

    public void TurnChecker()
    {
        redText.SetActive(false);
        blueText.SetActive(false);
        greenText.SetActive(false);
        purpleText.SetActive(false);

        switch (GameManager.playerTurn)
        {
            case 1:
                redText.SetActive(true);
                break;
            case 2:
                blueText.SetActive(true);
                break;
            case 3:
                greenText.SetActive(true);
                break;
            case 4:
                purpleText.SetActive(true);
                break;
        }
    }
}
