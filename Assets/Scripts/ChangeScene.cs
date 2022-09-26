using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class ChangeScene : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown playerSelect;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartGame()
    {
        switch (playerSelect.value)
        {
            case 0:
                GameManager.numberOfPlayers = 2;
                GameManager.maxGolems = 8;
                break;
            case 1:
                GameManager.numberOfPlayers = 3;
                GameManager.maxGolems = 12;
                break;
            case 2:
                GameManager.numberOfPlayers = 4;
                GameManager.maxGolems = 16;
                break;
        }

        Debug.Log("number of players: " + GameManager.numberOfPlayers);
        SceneManager.LoadScene(0);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void HandleInputData(int value)
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
