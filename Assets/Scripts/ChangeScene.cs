using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void HandleInputData(int value)
    {
        if (value == 0)
        {
            Debug.Log("VALUE 2" + value);
            // INSTANTIATE RED AND BLUE
        }
        if (value == 0)
        {
            Debug.Log("VALUE 3" + value);
            // INSTANTIATE RED BLUE AND GREEN
        }
        if (value == 0)
        {
            Debug.Log("VALUE 4" + value);
            // INSTANTIATE RED BLUE AND GREEN... AND PURPLE
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
